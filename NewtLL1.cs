using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Grimoire
{
    public sealed class NewtLL1 : IVsSingleFileGenerator
    {
        #region IVsSingleFileGenerator Members

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".cs";
            return pbstrDefaultExtension.Length;
        }

        private bool ReportMessage(IVsGeneratorProgress p, EbnfMessage m)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != p)
                    switch (m.ErrorLevel)
                    {
                        case EbnfErrorLevel.Warning:
                            p.GeneratorError(1, 1, m.Message, (uint)m.Line, (uint)m.Column);
                            return true;
                        case EbnfErrorLevel.Error:
                            p.GeneratorError(0, 1, m.Message, (uint)m.Line, (uint)m.Column);
                            return true;
                    }
            }
            catch
            {
            }

            return false;
        }

        private bool ReportMessage(IVsGeneratorProgress p, CfgMessage m)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != p)
                    switch (m.ErrorLevel)
                    {
                        case CfgErrorLevel.Warning:
                            p.GeneratorError(1, 1, m.Message, 0, 0);
                            return true;
                        case CfgErrorLevel.Error:
                            p.GeneratorError(0, 1, m.Message, 0, 0);
                            return true;
                    }
            }
            catch
            {
            }

            return false;
        }

        private bool ReportMessage(IVsGeneratorProgress p, string m, int line = 0, int column = 0, bool error = true)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != p)
                {
                    p.GeneratorError(error ? 0 : 1, 1, m, (uint)line, (uint)column);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private bool ReportProgress(IVsGeneratorProgress p, int step, int steps)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != p)
                {
                    p.Progress((uint)step, (uint)steps);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace, IntPtr[] rgbOutputFileContents,
            out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                using (var stm = new MemoryStream())
                {
                    ReportProgress(pGenerateProgress, 1, 4);
                    EbnfDocument doc = null;
                    var msgs = new List<object>();
                    var sw = new StreamWriter(stm);
                    using (var sr = new StreamReader(wszInputFilePath))
                    {
                        try
                        {
                            doc = EbnfDocument.ReadFrom(sr);
                        }
                        catch (ExpectingException ex)
                        {
                            var em = string.Concat("Error parsing grammar: ", ex.Message);
                            if (!ReportMessage(pGenerateProgress, em, ex.Line, ex.Column))
                                msgs.Add(em);
                            WriteHeader(sw, wszInputFilePath, msgs);
                            goto done;
                        }

                        ReportProgress(pGenerateProgress, 2, 4);
                    }

                    var hasErrors = false;
                    foreach (var m in doc.Prepare(false))
                    {
                        if (!ReportMessage(pGenerateProgress, m))
                            msgs.Add(m);
                        if (EbnfErrorLevel.Error == m.ErrorLevel)
                            hasErrors = true;
                    }

                    if (hasErrors)
                    {
                        // make sure to dump the messages
                        WriteHeader(sw, wszInputFilePath, msgs);
                        goto done;
                    }

                    var name = string.Concat(Path.GetFileNameWithoutExtension(wszInputFilePath), "Parser");
                    var cfg = doc.ToCfg();
                    foreach (var m in cfg.PrepareLL1(false))
                    {
                        if (!ReportMessage(pGenerateProgress, m))
                            msgs.Add(m);
                        if (CfgErrorLevel.Error == m.ErrorLevel)
                            hasErrors = true;
                    }

                    if (hasErrors)
                    {
                        WriteHeader(sw, wszInputFilePath, msgs);
                        goto done;
                    }

                    ReportProgress(pGenerateProgress, 3, 4);
                    var lexer = doc.ToLexer(cfg);
                    WriteHeader(sw, wszInputFilePath, msgs);
                    var hasNS = !string.IsNullOrEmpty(wszDefaultNamespace);
                    if (hasNS)
                        sw.WriteLine(string.Concat("namespace ", wszDefaultNamespace, " {"));
                    cfg.WriteCSharpTableDrivenLL1ParserClassTo(sw, name, null, lexer);
                    if (hasNS)
                        sw.WriteLine("}");
                    ReportProgress(pGenerateProgress, 4, 4);
                    done:
                    sw.Flush();
                    var length = (int)stm.Length;
                    rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(length);
                    Marshal.Copy(stm.GetBuffer(), 0, rgbOutputFileContents[0], length);
                    pcbOutput = (uint)length;
                }
            }
            catch (Exception ex)
            {
                var s = string.Concat("/* ", ex.Message, " */");
                var b = Encoding.UTF8.GetBytes(s);
                var length = b.Length;
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(length);
                Marshal.Copy(b, 0, rgbOutputFileContents[0], length);
                pcbOutput = (uint)length;
            }

            return VSConstants.S_OK;
        }

        public static void WriteHeader(TextWriter writer, string wszInputFilePath, IEnumerable<object> msgs)
        {
            if (null != wszInputFilePath)
                writer.WriteLine(string.Concat("#line 1 \"", Path.GetFullPath(wszInputFilePath).Replace("\"", "\"\""),
                    "\""));
            foreach (var m in msgs)
            {
                var em = m as EbnfMessage;
                if (null != em)
                {
                    if (EbnfErrorLevel.Warning == em.ErrorLevel)
                        writer.Write("#warning ");
                    else if (EbnfErrorLevel.Error == em.ErrorLevel)
                        writer.Write("#error ");
                    else if (EbnfErrorLevel.Message == em.ErrorLevel)
                        writer.Write("// Generator ");
                    else
                        continue;
                    writer.WriteLine(em.ToString());
                }
                else
                {
                    var cm = m as CfgMessage;
                    if (null != cm)
                    {
                        if (CfgErrorLevel.Warning == cm.ErrorLevel)
                            writer.Write("#warning ");
                        else if (CfgErrorLevel.Error == cm.ErrorLevel)
                            writer.Write("#error ");
                        else if (CfgErrorLevel.Message == cm.ErrorLevel)
                            writer.Write("// Generator ");
                        else
                            continue;
                        writer.WriteLine(cm.ToString());
                    }
                    else
                    {
                        writer.WriteLine(string.Concat("#error ", m));
                    }
                }
            }
        }

        #endregion
    }
}