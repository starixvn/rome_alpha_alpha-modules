namespace Alpha.NetThrottling.Utils
{
    using System;

    public interface Clock
    {
        DateTime Now { get; }

        DateTime MinValue { get; }
    }

    public class SystemClock : Clock
    {
        public DateTime Now => DateTime.Now;

        public DateTime MinValue => DateTime.MinValue;
    }
}
