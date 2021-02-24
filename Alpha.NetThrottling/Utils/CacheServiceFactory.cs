namespace Alpha.NetThrottling.Utils
{
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using System.Collections.Generic;

    public interface ICacheServiceFactory
    {
        ICacheService Create(RedisCacheOptions options);
    }

    public class CacheServiceFactory : ICacheServiceFactory
    {
        public static readonly ICacheServiceFactory Default = new CacheServiceFactory();
        private readonly Dictionary<string, ICacheService> cacheServices = new Dictionary<string, ICacheService>();

        public ICacheService Create(RedisCacheOptions options)
        {
            if (!cacheServices.ContainsKey(options.ApplicationName))
            {
                lock (cacheServices)
                {
                    var cacheService = new CacheService(new RedisCache(options));
                    cacheServices.Add(options.ApplicationName, cacheService);
                    return cacheService;
                }
            }

            return cacheServices[options.ApplicationName];
        }
    }
}
