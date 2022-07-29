namespace Grimoire
{
    internal sealed class EbnfMessage
    {
        public EbnfMessage(EbnfErrorLevel errorLevel, int errorCode, string message, int line, int column,
            long position)
        {
            ErrorLevel = errorLevel;
            ErrorCode = errorCode;
            Message = message;
            Line = line;
            Column = column;
            Position = position;
        }

        public EbnfErrorLevel ErrorLevel { get; }
        public int ErrorCode { get; }
        public string Message { get; }
        public int Line { get; }
        public int Column { get; }
        public long Position { get; }

        public override string ToString()
        {
            if (-1 == Position)
            {
                if (-1 != ErrorCode)
                    return string.Format("{0}: {1} code {2}",
                        ErrorLevel, Message, ErrorCode);
                return string.Format("{0}: {1}",
                    ErrorLevel, Message);
            }

            if (-1 != ErrorCode)
                return string.Format("{0}: {1} code {2} at line {3}, column {4}, position {5}",
                    ErrorLevel, Message, ErrorCode, Line, Column, Position);
            return string.Format("{0}: {1} at line {2}, column {3}, position {4}",
                ErrorLevel, Message, Line, Column, Position);
        }
    }
}