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
            var a = new DateTime();
            cacheService.TryGet(blockKey, out a);
            var (isHasValue, _) = await cacheService.TryGetAsync<DateTime>(blockKey);
            return isHasValue;
        }

        private async Task AddBlacklistAsync(string blockKey, int blockDuration)
        {
            await cacheService.SetAsync(
                blockKey,
                clock.Now, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                });
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext);
            return await ThrottleAsync(policy, cacheIdentity);
        }

        public async Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            if (await IsBlacklistAsync(cacheIdentity.BlockKey))
            {
                return ThrottleResult.Blocked;
            }

            var (isHasValue, throttleCounter) = await cacheService.TryGetAsync<ThrottleCounter>(cacheIdentity.ThrottleKey);
            throttleCounter = !isHasValue || clock.Now > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.ThrottleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = clock.Now,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > policy.MaxRequests)
            {
                await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration);
                return ThrottleResult.Blocked;
            }

            await cacheService.SetAsync(
                cacheIdentity.ThrottleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.ThrottleDuration)
                });
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
            return await ThrottleAsync(work, blocked, policy, cacheIdentity);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            if (await IsBlacklistAsync(cacheIdentity.BlockKey))
            {
                return blocked();
            }

            async Task<TResult> success(TResult returnType)
            {
                var throttleResult = await ThrottleAsync(policy, cacheIdentity);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration);
                    return blocked();
                }

                return returnType;
            }

            return await work(success);
        }
    }
}
