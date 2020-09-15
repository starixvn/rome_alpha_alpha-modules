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

        public static CacheIdentity PathAndKey(string path, string key)
        {
            var throttleKey = $"{path}}}:{{{Constants.Throttle}}}:{{{key}";
            var blockKey = $"{path}}}:{{{Constants.Block}}}:{{{key}";
            return new CacheIdentity(throttleKey, blockKey);
        }
    }
}
