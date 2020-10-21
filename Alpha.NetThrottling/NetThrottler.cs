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
        IThrottleSetting ThrottleSetting { get; }

        bool IsBlacklist(CacheIdentity cacheIdentity);

        void RemoveBlacklist(CacheIdentity cacheIdentity);

        void RemoveThrottling(CacheIdentity cacheIdentity);

        void AddBlacklist(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        TimeSpan GetBlockRemainingTime(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter);

        ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter, bool blockImmediately);

        ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately);

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
        Task<bool> IsBlacklistAsync(CacheIdentity cacheIdentity);

        Task RemoveBlacklistAsync(CacheIdentity cacheIdentity);

        Task RemoveThrottlingAsync(CacheIdentity cacheIdentity);

        Task AddBlacklistAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<TimeSpan> GetBlockRemainingTimeAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter);

        Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter, bool blockImmediately);

        Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately);

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
        private readonly IClock clock;

        public IThrottleSetting ThrottleSetting { get; private set; }

        public ICacheServiceWrapper CacheService
        {
            get
            {
                return ThrottleSetting.CacheService;
            }
        }
        public NetThrottler()
            : this(null, null)
        {
        }

        internal NetThrottler(IThrottleSetting throttleSetting)
            : this(throttleSetting, null)
        {
        }

        internal NetThrottler(IThrottleSetting throttleSetting, IClock clock, ILogger logger)
        {
            ThrottleSetting = throttleSetting ?? new ThrottleSetting();
            this.clock = clock ?? new Clock();
        }

        public bool IsBlacklist(CacheIdentity cacheIdentity)
        {
            return CacheService.Contain(cacheIdentity.BlockKey);
        }

        public void RemoveBlacklist(CacheIdentity cacheIdentity)
        {
            CacheService.Remove(cacheIdentity.BlockKey);
        }

        public void RemoveThrottling(CacheIdentity cacheIdentity)
        {
            CacheService.Remove(cacheIdentity.ThrottleKey);
        }

        public void AddBlacklist(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            CacheService.Set(
                cacheIdentity.BlockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(policy.BlockDuration)
                });
        public TimeSpan GetBlockRemainingTime(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            var isHasValue = CacheService.TryGet(cacheIdentity.BlockKey, out DateTime blockTime);
            if (isHasValue)
            {
                return blockTime.AddSeconds(policy.BlockDuration) - clock.UtcNow;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
            return Throttle(httpContext, policy, filter, false);
        }

        public ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter, bool blockImmediately)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return Throttle(policy, cacheIdentity, blockImmediately);
        public ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity)
            return Throttle(policy, cacheIdentity, false);
        }

        public ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately)
        {
            if (cacheIdentity == null)
                return ThrottleResult.Allowed;
            if (IsBlacklist(cacheIdentity))
            {
                return ThrottleResult.Blocked;
            }

            var isHasValue = CacheService.TryGet(cacheIdentity.ThrottleKey, out ThrottleCounter throttleCounter);
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

            if (throttleCounter.TotalRequests > policy.MaxRequests ||
                (blockImmediately && throttleCounter.TotalRequests == policy.MaxRequests))
            {
                AddBlacklist(policy, cacheIdentity);
                RemoveThrottling(cacheIdentity);
                return ThrottleResult.Blocked;
            }

            CacheService.Set(
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
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return Throttle(work, blocked, policy, cacheIdentity);
        }

        public TResult Throttle<TResult>(
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            TResult success(TResult returnType)
            {
                var throttleResult = Throttle(policy, cacheIdentity);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    AddBlacklist(policy, cacheIdentity);
                    RemoveThrottling(cacheIdentity);
                    return blocked();
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return work(success);
            }

            if (IsBlacklist(cacheIdentity))
            {
                return blocked();
            }

            return work(success);
        }

        public async Task<bool> IsBlacklistAsync(CacheIdentity cacheIdentity)
        {
            return await CacheService.ContainAsync(cacheIdentity.BlockKey).ConfigureAwait(false);
        }

        public async Task RemoveBlacklistAsync(CacheIdentity cacheIdentity)
        {
            await CacheService.RemoveAsync(cacheIdentity.BlockKey).ConfigureAwait(false);
        }

        public async Task RemoveThrottlingAsync(CacheIdentity cacheIdentity)
        {
            await CacheService.RemoveAsync(cacheIdentity.ThrottleKey).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetBlockRemainingTimeAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            var (isHasValue, blockTime) = await CacheService.TryGetAsync<DateTime>(cacheIdentity.BlockKey).ConfigureAwait(false);
            if (isHasValue)
            {
                return blockTime.AddSeconds(policy.BlockDuration) - clock.UtcNow;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public async Task AddBlacklistAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            await CacheService.SetAsync(
                cacheIdentity.BlockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(policy.BlockDuration)
                }).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            return await ThrottleAsync(httpContext, policy, filter, false).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter, bool blockImmediately)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            return await ThrottleAsync(policy, cacheIdentity, false).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            if (await IsBlacklistAsync(cacheIdentity).ConfigureAwait(false))
            {
                return ThrottleResult.Blocked;
            }

            var (isHasValue, throttleCounter) = await CacheService.TryGetAsync<ThrottleCounter>(cacheIdentity.ThrottleKey).ConfigureAwait(false);
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

            if (throttleCounter.TotalRequests > policy.MaxRequests ||
                (blockImmediately && throttleCounter.TotalRequests == policy.MaxRequests))
            {
                await AddBlacklistAsync(policy, cacheIdentity).ConfigureAwait(false);
                await RemoveThrottlingAsync(cacheIdentity).ConfigureAwait(false);
                return ThrottleResult.Blocked;
            }

            await CacheService.SetAsync(
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
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return await ThrottleAsync(work, blocked, policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            async Task<TResult> success(TResult returnType)
            {
                var throttleResult = await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    await AddBlacklistAsync(policy, cacheIdentity).ConfigureAwait(false);
                    await RemoveThrottlingAsync(cacheIdentity).ConfigureAwait(false);
                    return blocked();
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return await work(success).ConfigureAwait(false);
            }

            if (await IsBlacklistAsync(cacheIdentity).ConfigureAwait(false))
            {
                return blocked();
            }

            return await work(success).ConfigureAwait(false);
        }
    }
}
