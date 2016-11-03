namespace Sunplus.Agent.Utilities
{
    using System;

    public class LoggerImpl : Logger
    {
        public void FilterInfoWarnOrError(Exception ex, int customerId = 0)
        {
            Logs.FilterInfoWarnOrError(ex, 0);
        }

        public string FullExceptionToString(Exception ex, int customerId = 0)
        {
            return Logs.FullExceptionToString(ex, customerId);
        }

        public void Write(Exception ex, int customerId = 0)
        {
            Logs.Write(ex, customerId);
        }

        public void WriteInfo(string message)
        {
            Logs.WriteInfo(message);
        }

        public void WriteInfo(Exception ex, int customerId = 0)
        {
            Logs.WriteInfo(ex, customerId);
        }

        public void WriteWarn(Exception ex, int customerId = 0)
        {
            Logs.WriteWarn(ex, customerId);
        }
    }
}