namespace Grimoire
{
    internal sealed class CfgMessage
    {
        public CfgMessage(CfgErrorLevel errorLevel, int errorCode, string message)
        {
            ErrorLevel = errorLevel;
            ErrorCode = errorCode;
            Message = message;
        }

        public CfgErrorLevel ErrorLevel { get; }
        public int ErrorCode { get; }
        public string Message { get; }

        public override string ToString()
        {
            if (-1 != ErrorCode)
                return string.Format("{0}: {1} code {2}",
                    ErrorLevel, Message, ErrorCode);
            return string.Format("{0}: {1}",
                ErrorLevel, Message);
        }
    }
}