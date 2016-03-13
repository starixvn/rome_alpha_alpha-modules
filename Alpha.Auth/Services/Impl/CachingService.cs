namespace Alpha.Auth.Services.Impl
{
    using System;
    using System.Runtime.Caching;

    internal class CachingService : ICachingService
    {
        private static volatile ObjectCache _cacheManager;

        private static object _syncRoot = new object();

        protected ObjectCache CacheManager
        {
            get
            {
                if (_cacheManager == null)
                {
                    lock (_syncRoot)
                    {
                        if (_cacheManager == null)
                        {
                            _cacheManager = MemoryCache.Default;
                        }
                    }
                }

                return _cacheManager;
            }
        }

        public T GetOrAdd<T>(string key, Func<T> addItem, int expiredSeconds = 30)
        {
            var value = CacheManager.Get(key);

            if (value == null)
            {
                value = addItem();

                CacheManager.Set(
                    key,
                    value,
                    new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(expiredSeconds)
                    });
            }

            if (value is T)
            {
                return (T)value;
            }

            return default(T);
        }
    }
}