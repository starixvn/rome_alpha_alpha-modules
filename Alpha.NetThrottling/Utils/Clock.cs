namespace Alpha.NetThrottling.Utils
{
    using System;

    public interface IClock
    {
        DateTime Now { get; }

        DateTime MinValue { get; }
    }

    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;

        public DateTime MinValue => DateTime.MinValue;
    }
}
