namespace Alpha.Auth.Services
{
    using System;

    public interface ICachingService
    {
        T GetOrAdd<T>(string key, Func<T> addItem, int expiredSeconds = 30);
    }
}