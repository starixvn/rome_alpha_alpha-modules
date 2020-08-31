namespace Alpha.NetThrottling.Models
{
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IThrottleSetting
    {
        string CacheKeyPrefix { get; }

        IResolver Resolver { get; }

        IEnumerable<NetThrottlingKeyElement> Keys { get; }

        NetThrottlingKeyElement GetKey(string name);
    }

    public class ThrottleSetting : IThrottleSetting
    {
        public string CacheKeyPrefix { get; private set; }

        public IResolver Resolver { get; private set; }

        public IEnumerable<NetThrottlingKeyElement> Keys { get; private set; }

        public ThrottleSetting()
            :this(NetThrottlingConfiguration.Default)
        {
        }

        public ThrottleSetting(string sectionName)
            :this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public ThrottleSetting(NetThrottlingConfiguration configuration)
        {
            CacheKeyPrefix = !string.IsNullOrWhiteSpace(configuration.Prefix)
                ? configuration.Prefix
                : HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name;
            Resolver = string.IsNullOrEmpty(configuration.Resolver) ? new BaseResolver() : TypeFactory<IResolver>.Create(configuration.Resolver);
            Keys = configuration.Keys.Cast<NetThrottlingKeyElement>();
        }

        public ThrottleSetting(string cacheKeyPrefix, IResolver resolver, IEnumerable<NetThrottlingKeyElement> keys)
        {
            CacheKeyPrefix = cacheKeyPrefix;
            Resolver = resolver;
            Keys = keys;
        }

        public NetThrottlingKeyElement GetKey(string name)
        {
            return Keys.FirstOrDefault(k => k.Name == name);
        }
    }
}
