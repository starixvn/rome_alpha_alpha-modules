namespace Alpha.NetThrottling.Utils
{
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Caching.Redis;
    using System.Collections.Generic;

    public interface ICacheServiceFactory
    {
        ICacheServiceWrapper Create(RedisCacheOptions options);

        ICacheServiceWrapper Create();
    }

    public class CacheServiceFactory : ICacheServiceFactory
    {
        public static readonly ICacheServiceFactory Default = new CacheServiceFactory();
        private readonly Dictionary<string, ICacheServiceWrapper> cacheServices = new Dictionary<string, ICacheServiceWrapper>();
        private const string MemoryCacheKey = "MemoryCacheKey";

        public ICacheServiceWrapper Create(RedisCacheOptions options)
        {
            if (!cacheServices.ContainsKey(options.ApplicationName))
            {
                lock (cacheServices)
                {
                    var cacheService = new CacheServiceWrapper(new CacheService(new RedisCache(options)));
                    cacheServices.Add(options.ApplicationName, cacheService);
                    return cacheService;
                }
            }

            return cacheServices[options.ApplicationName];
        }

        public ICacheServiceWrapper Create()
        {
            if (!cacheServices.ContainsKey(MemoryCacheKey))
            {
                lock (cacheServices)
                {
                    var cacheService = new CacheServiceWrapper(new CacheService(new MemoryCache(new MemoryCacheOptions())));
                    cacheServices.Add(MemoryCacheKey, cacheService);
                    return cacheService;
                }
            }

            return cacheServices[MemoryCacheKey];
        }
    }
}
