using System;
using System.Collections.Generic;

namespace Grimoire
{
    internal sealed class CfgException : Exception
    {
        public CfgException(string message, int errorCode = -1) :
            this(new[] { new CfgMessage(CfgErrorLevel.Error, errorCode, message) })
        {
        }

        public CfgException(IEnumerable<CfgMessage> messages) : base(_FindMessage(messages))
        {
            Messages = new List<CfgMessage>(messages);
        }

        public IList<CfgMessage> Messages { get; }

        private static string _FindMessage(IEnumerable<CfgMessage> messages)
        {
            var l = new List<CfgMessage>(messages);
            if (null == messages) return "";
            var c = 0;
            foreach (var m in l)
            {
                if (CfgErrorLevel.Error == m.ErrorLevel)
                {
                    if (1 == l.Count)
                        return m.ToString();
                    return string.Concat(m, " (multiple messages)");
                }

                ++c;
            }

            foreach (var m in messages)
                return m.ToString();
            return "";
        }

        public static void ThrowIfErrors(IEnumerable<CfgMessage> messages)
        {
            if (null == messages) return;
            foreach (var m in messages)
                if (CfgErrorLevel.Error == m.ErrorLevel)
                    throw new CfgException(messages);
        }
    }
}