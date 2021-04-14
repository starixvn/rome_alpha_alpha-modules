namespace Alpha.DNAide.Web
{
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System.Diagnostics;
    using System.Web;

    public static class DNAideLog
    {
        public static void Write(string msg)
        {
            Write(msg, TraceEventType.Verbose);
        }

        public static void Write(string msg, TraceEventType type)
        {
            try
            {
                StackFrame frame = (new StackTrace()).GetFrame(1);
                frame.GetMethod();
                string empty = string.Empty;
                try
                {
                    string[] appRelativeCurrentExecutionFilePath = new string[] { "[", HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "] -- [", frame.GetMethod().ToString(), "] -- " };
                    empty = string.Concat(appRelativeCurrentExecutionFilePath);
                }
                catch
                {
                    empty = "[could not get file/function name] -- ";
                }
                LogEntry logEntry = new LogEntry();
                logEntry.Message = string.Concat(empty, msg.ToString());
                logEntry.Severity = type;
                logEntry.TimeStamp = logEntry.TimeStamp.ToLocalTime();
                Logger.Write(logEntry);
            }
            catch
            {
            }
        }
    }
}
