namespace Alpha.NetThrottling.Models
{
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using StackExchange.Redis;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public interface IThrottleSetting
    {
        IResolver Resolver { get; }

        ICacheService CacheService { get; }

        IEnumerable<NetThrottlingKeyElement> Keys { get; }

        NetThrottlingKeyElement GetKey(string name);
    }

    public class ThrottleSetting : IThrottleSetting
    {
        public IResolver Resolver { get; private set; }

        public ICacheService CacheService { get; private set; }

        public IEnumerable<NetThrottlingKeyElement> Keys { get; private set; }

        public ThrottleSetting()
            : this(NetThrottlingConfiguration.Default)
        {
        }

        public ThrottleSetting(string sectionName)
            : this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public ThrottleSetting(NetThrottlingConfiguration configuration)
        {
            Resolver = string.IsNullOrEmpty(configuration.Resolver) ? new BaseResolver() : TypeFactory<IResolver>.Create(configuration.Resolver);
            Keys = configuration.Keys.Cast<NetThrottlingKeyElement>();
            var redisConfigurationString = ConfigurationManager.AppSettings[configuration.RedisConfigurationOptionsKey];
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationString);
            var redisCacheOptions = new RedisCacheOptions
            {
                ApplicationName = configurationOptions.ClientName,
                ConnectionOptions = configurationOptions
            };
            CacheService = new CacheService(new RedisCache(redisCacheOptions));
        }

        public ThrottleSetting(IResolver resolver, ICacheService cacheService, IEnumerable<NetThrottlingKeyElement> keys)
        {
            Resolver = resolver;
            CacheService = cacheService;
            Keys = keys;
        }

        public NetThrottlingKeyElement GetKey(string name)
        {
            return Keys.FirstOrDefault(k => k.Name == name);
        }
    }
}
