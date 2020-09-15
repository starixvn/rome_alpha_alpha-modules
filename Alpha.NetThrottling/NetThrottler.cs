namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public interface INetThrottler
    {
        ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter);

        ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        TResult Throttle<TResult>(
            HttpContextBase httpContext,
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter);

        TResult Throttle<TResult>(
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity);

        Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter);

        Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<TResult> ThrottleAsync<TResult>(
            HttpContextBase httpContext,
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter);

        Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity);
    }

    public class NetThrottler : INetThrottler
    {
        private readonly ICacheService cacheService;
        private readonly IClock clock;

        public NetThrottler()
            : this(null, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting)
            : this(throttleSetting, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting, ICacheService cacheService)
            : this(throttleSetting, cacheService, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting, ICacheService cacheService, IClock clock)
        {
            throttleSetting = throttleSetting ?? new ThrottleSetting(NetThrottlingConfiguration.Default);
            this.cacheService = cacheService ?? throttleSetting.CacheService;
            this.clock = clock ?? new Clock();
        }

        private async Task<bool> IsBlacklistAsync(string blockKey)
        {
            return await cacheService.ContainAsync(blockKey).ConfigureAwait(false);
        }

        private bool IsBlacklist(string blockKey)
        {
            return cacheService.Contain(blockKey);
        }

        private async Task AddBlacklistAsync(string blockKey, int blockDuration)
        {
            await cacheService.SetAsync(
                blockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                }).ConfigureAwait(false);
        }

        private void AddBlacklist(string blockKey, int blockDuration)
        {
            cacheService.Set(
                blockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                });
        }

        public ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext);
            return Throttle(policy, cacheIdentity);
        }

        public ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            if (IsBlacklist(cacheIdentity.BlockKey))
            {
                return ThrottleResult.Blocked;
            }

            var isHasValue = cacheService.TryGet(cacheIdentity.ThrottleKey, out ThrottleCounter throttleCounter);
            throttleCounter = !isHasValue || clock.UtcNow > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.ThrottleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = clock.UtcNow,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > policy.MaxRequests)
            {
                AddBlacklist(cacheIdentity.BlockKey, policy.BlockDuration);
                return ThrottleResult.Blocked;
            }

            cacheService.Set(
                cacheIdentity.ThrottleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.ThrottleDuration)
                });
            return ThrottleResult.Allowed;
        }

        public TResult Throttle<TResult>(
            HttpContextBase httpContext,
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext);
            return Throttle(work, blocked, policy, cacheIdentity);
        }

        public TResult Throttle<TResult>(
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return work(success);
            }

            if (IsBlacklist(cacheIdentity.BlockKey))
            {
                return blocked();
            }

            TResult success(TResult returnType)
            {
                var throttleResult = Throttle(policy, cacheIdentity);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    AddBlacklist(cacheIdentity.BlockKey, policy.BlockDuration);
                    return blocked();
                }

                return returnType;
            }

            return work(success);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext);
            return await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            if (await IsBlacklistAsync(cacheIdentity.BlockKey).ConfigureAwait(false))
            {
                return ThrottleResult.Blocked;
            }

            var (isHasValue, throttleCounter) = await cacheService.TryGetAsync<ThrottleCounter>(cacheIdentity.ThrottleKey).ConfigureAwait(false);
            throttleCounter = !isHasValue || clock.UtcNow > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.ThrottleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = clock.UtcNow,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > policy.MaxRequests)
            {
                await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration).ConfigureAwait(false);
                return ThrottleResult.Blocked;
            }

            await cacheService.SetAsync(
                cacheIdentity.ThrottleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.ThrottleDuration)
                }).ConfigureAwait(false);
            return ThrottleResult.Allowed;
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            HttpContextBase httpContext,
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext);
            return await ThrottleAsync(work, blocked, policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return await work(success).ConfigureAwait(false);
            }

            if (await IsBlacklistAsync(cacheIdentity.BlockKey).ConfigureAwait(false))
            {
                return blocked();
            }

            async Task<TResult> success(TResult returnType)
            {
                var throttleResult = await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration).ConfigureAwait(false);
                    return blocked();
                }

                return returnType;
            }

            return await work(success).ConfigureAwait(false);
        }
    }
}
