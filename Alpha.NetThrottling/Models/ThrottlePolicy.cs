namespace Alpha.NetThrottling.Models
{
    public class ThrottlePolicy
    {
        public string Name { get; set; }

        public int MaxRequests { get; set; }

        public int ThrottleDuration { get; set; }

        public int BlockDuration { get; set; }

        public string Filter { get; set; }
    }
}
