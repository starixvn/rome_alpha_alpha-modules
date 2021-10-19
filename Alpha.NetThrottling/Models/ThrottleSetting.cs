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

        ICacheServiceWrapper CacheService { get; }

        IEnumerable<NetThrottlingKeyElement> Keys { get; }

        NetThrottlingKeyElement GetKey(string name);
    }

    public class ThrottleSetting : IThrottleSetting
    {
        public IResolver Resolver { get; private set; }

        public ICacheServiceWrapper CacheService { get; private set; }

        public IEnumerable<NetThrottlingKeyElement> Keys { get; private set; }

        public ThrottleSetting(bool isUsedRedis)
            : this(null, null, isUsedRedis)
        {
        }

        internal ThrottleSetting(NetThrottlingConfiguration configuration, ICacheServiceFactory cacheServiceFactory, bool isUsedRedis)
        {
            configuration = configuration ?? NetThrottlingConfiguration.Default;
            cacheServiceFactory = cacheServiceFactory ?? CacheServiceFactory.Default;
            Resolver = string.IsNullOrEmpty(configuration.Resolver) ? new BaseResolver() : TypeFactory<IResolver>.Create(configuration.Resolver);
            Keys = configuration.Keys.Cast<NetThrottlingKeyElement>();
            var redisConfigurationString = ConfigurationManager.AppSettings[configuration.RedisConfigurationOptionsKey];
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationString);
            var redisCacheOptions = new RedisCacheOptions
            {
                ApplicationName = configurationOptions.ClientName,
                ConnectionOptions = configurationOptions
            };

            if (isUsedRedis)
            {
                CacheService = cacheServiceFactory.Create(redisCacheOptions);
            }
            else
            {
                CacheService = cacheServiceFactory.Create();
            }
        }

        public NetThrottlingKeyElement GetKey(string name)
        {
            return Keys.FirstOrDefault(k => k.Name == name);
        }
    }
}
