namespace Alpha.NetThrottling.Models
{
    using System;

    [Serializable]
    public class ThrottleCounter
    {
        public DateTime Timestamp { get; set; }

        public int TotalRequests { get; set; }
    }
}
