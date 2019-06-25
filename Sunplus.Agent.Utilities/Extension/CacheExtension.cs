namespace Sunplus.Agent.Utilities.Extension
{
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Enyim.Caching.Memcached.Results;
    using System;

    /// <summary>
    /// Cache Extension.
    /// </summary>
    public static class CacheExtension
    {
        private static CouchbaseClient _couchbaseClient = new CouchbaseClient();

        public static object Get(string key)
        {
            return _couchbaseClient.Get(key);
        }

        public static T Get<T>(string key)
        {
            return _couchbaseClient.Get<T>(key);
        }

        /// <summary>
        /// Function to set cache info.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="value">Cache value.</param>
        /// <param name="expireTime">Expire time [Second].</param>
        public static IStoreOperationResult Set(string key, object value, int expireTime = 0)
        {
            if (expireTime == 0)
            {
                return _couchbaseClient.ExecuteStore(StoreMode.Set, key, value);
            }

            return _couchbaseClient.ExecuteStore(StoreMode.Set, key, value, DateTime.Now.AddSeconds(expireTime));
        }

        public static void Remove(string key)
        {
            _couchbaseClient.Remove(key);
        }
    }
}
