namespace Alpha.NetThrottling.Utils
{
    using Alpha.NetThrottling.Models;

    public static class Mapper
    {
        public static ThrottlePolicy From(NetThrottlingKeyElement key)
        {
            return new ThrottlePolicy
            {
                Name = key.Name,
                MaxRequests = key.MaxRequests,
                BlockDuration = key.BlockDuration,
                ThrottleDuration = key.ThrottleDuration,
                Filter = key.Filter
            };
        }
    }
}
