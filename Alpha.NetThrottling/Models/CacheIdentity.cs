namespace Alpha.NetThrottling.Models
{
    using Alpha.NetThrottling.Utils;
    using Alpha.Web.Configurations;
    using System;
    using System.Linq;

    public class CacheIdentity
    {
        public string ThrottleKey { get; private set; }

        public string BlockKey { get; private set; }

        public CacheIdentity(string throttleKey, string blockKey)
        {
            ThrottleKey = throttleKey;
            BlockKey = blockKey;
        }

        public static CacheIdentity From(IThrottlePolicy policy, string[] keys)
        {
            return From(policy.Name, AppSettings.SiteLabel, keys);
        }

        public static CacheIdentity From(string policyName, string siteLabel, string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var prefix = $"{policyName}}}:{{{siteLabel}}}";
            var suffix = $"{{{string.Join("_", keys)}";
            var throttleKey = $"{prefix}:{{{Constants.Throttle}}}:{suffix}";
            var blockKey = $"{prefix}:{{{Constants.Block}}}:{suffix}";
            return new CacheIdentity(throttleKey, blockKey);
        }
    }
}
