namespace Alpha.NetThrottling.Utils
{
    using Fanex.Caching;
    using Fanex.Caching.Memory;

    public static class CacheFactory
    {
        private static CacheService defaultCacheService;

        public static CacheService CreateDefaultService()
        {
            defaultCacheService = defaultCacheService ?? new CacheServiceImpl(new MemoryCache());
            return defaultCacheService;
        }
    }
}
