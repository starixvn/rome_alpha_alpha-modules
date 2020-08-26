namespace Alpha.NetThrottling.Models
{
    using System;

    public interface IThrottlePolicy
    {
        string Name { get; }

        int MaxRequests { get; }

        int ThrottleDuration { get; }

        int BlockDuration { get; }
    }

    public class ThrottlePolicy : IThrottlePolicy
    {
        public ThrottlePolicy(NetThrottlingKeyElement key)
            : this(key.Name,
                  key.MaxRequests,
                  key.BlockDuration,
                  key.ThrottleDuration)
        {
        }

        public ThrottlePolicy(
            string name,
            int maxRequests,
            int blockDuration,
            int throttleDuration)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (maxRequests <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRequests));
            }

            if (blockDuration <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(blockDuration));
            }

            if (throttleDuration <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(throttleDuration));
            }

            Name = name;
            MaxRequests = maxRequests;
            BlockDuration = blockDuration;
            ThrottleDuration = throttleDuration;
        }

        public string Name { get; private set; }

        public int MaxRequests { get; private set; }

        public int ThrottleDuration { get; private set; }

        public int BlockDuration { get; private set; }
    }
}
