namespace Sunplus.Agent.Utilities
{
    using System;

    public interface Logger
    {
        void FilterInfoWarnOrError(Exception ex, int customerId = 0);

        void Write(Exception ex, int customerId = 0);

        void WriteInfo(string message);

        void WriteInfo(Exception ex, int customerId = 0);

        void WriteWarn(Exception ex, int customerId = 0);

        string FullExceptionToString(Exception ex, int customerId = 0);
    }
}