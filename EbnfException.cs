using System;
using System.Collections.Generic;

namespace Grimoire
{
    internal sealed class EbnfException : Exception
    {
        public EbnfException(string message, int errorCode = -1, int line = 0, int column = 0, long position = -1) :
            this(new[] { new EbnfMessage(EbnfErrorLevel.Error, errorCode, message, line, column, position) })
        {
        }

        public EbnfException(IEnumerable<EbnfMessage> messages) : base(_FindMessage(messages))
        {
            Messages = new List<EbnfMessage>(messages);
        }

        public IList<EbnfMessage> Messages { get; }

        private static string _FindMessage(IEnumerable<EbnfMessage> messages)
        {
            var l = new List<EbnfMessage>(messages);
            if (null == messages) return "";
            var c = 0;
            foreach (var m in l)
            {
                if (EbnfErrorLevel.Error == m.ErrorLevel)
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

        public static void ThrowIfErrors(IEnumerable<EbnfMessage> messages)
        {
            if (null == messages) return;
            foreach (var m in messages)
                if (EbnfErrorLevel.Error == m.ErrorLevel)
                    throw new EbnfException(messages);
        }
    }
}