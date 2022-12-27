namespace Alpha.Auth.Services
{
    using System;

    public interface ICachingService
    {
        T GetOrAdd<T>(string key, Func<T> addItem, int expiredSeconds = 30);

        T Get<T>(string key);

        void Set(string key, object value, int expiredSeconds = 30);

        void Remove(string key);
    }
}