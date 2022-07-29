using System;

namespace Grimoire
{
    /// <summary>
    ///     An exception encountered during parsing where the stream contains one thing, but another is expected
    /// </summary>
    [Serializable]
    internal sealed class ExpectingException : Exception
    {
        /// <summary>
        ///     Initialize the exception with the specified message.
        /// </summary>
        /// <param name="message">The message</param>
        public ExpectingException(string message) : base(message)
        {
        }

        /// <summary>
        ///     The list of expected strings.
        /// </summary>
        public string[] Expecting { get; internal set; }

        /// <summary>
        ///     The position when the error was realized.
        /// </summary>
        public long Position { get; internal set; }

        /// <summary>
        ///     The line of the error
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        ///     The column of the error
        /// </summary>
        public int Column { get; internal set; }
    }
}