namespace Alpha.NetThrottling.Models
{
    using Alpha.NetThrottling.Utils;

    public class CacheIdentity
    {
        public string ThrottleKey { get; private set; }

        public string BlockKey { get; private set; }

        public CacheIdentity(string throttleKey, string blockKey)
        {
            ThrottleKey = throttleKey;
            BlockKey = blockKey;
        }

        public static CacheIdentity From(string path, string key, IThrottlePolicy policy)
        {
            var prefix = $"{policy.Name}}}:{{{path}}}";
            var suffix = $"{{{key}";
            var throttleKey = $"{prefix}:{{{Constants.Throttle}}}:{suffix}";
            var blockKey = $"{prefix}:{{{Constants.Block}}}:{suffix}";
            return new CacheIdentity(throttleKey, blockKey);
        }
    }
}
