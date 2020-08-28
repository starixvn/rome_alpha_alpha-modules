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
        Task<ThrottleResult> ThrottleAsync(string filterKey, int maxRequests, int throttleDuration, int blockDuration);

        Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, IThrottlePolicy policy, IFilter filter);

        Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter);

        Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            string filterIdentity);
    }

    public class NetThrottler : INetThrottler
    {
        private const string BLOCK_PREFIX = "Block";
        private const string THROTTLE_PREFIX = "Throttle";

        private readonly IThrottleSetting throttleSetting;
        private readonly CacheService cacheService;
        private readonly Clock clock;

        public NetThrottler()
            : this(null, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting)
            : this(throttleSetting, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting, CacheService cacheService)
            : this(throttleSetting, cacheService, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting, CacheService cacheService, Clock clock)
        {
            this.throttleSetting = throttleSetting ?? new ThrottleSetting(NetThrottlingConfiguration.Default);
            this.cacheService = cacheService ?? CacheFactory.CreateDefaultService();
            this.clock = clock ?? new SystemClock();
        }

        private string GenerateThrottleKey(string filterKey)
        {
            return $"{throttleSetting.CacheKeyPrefix}_{THROTTLE_PREFIX}_{filterKey}";
        }

        private string GenerateBlockKey(string filterKey)
        {
            return $"{throttleSetting.CacheKeyPrefix}_{BLOCK_PREFIX}_{filterKey}";
        }

        private async Task<bool> IsBlacklistAsync(string filterKey)
        {
            var blockKey = GenerateBlockKey(filterKey);
            var blockValue = await cacheService.GetAsync<DateTime>(blockKey);
            if (blockValue != clock.MinValue)
            {
                return true;
            }

            return false;
        }

        private async Task AddBlacklistAsync(string filterKey, int blockDuration)
        {
            var blockKey = GenerateBlockKey(filterKey);
            await cacheService.SetAsync(
                blockKey,
                clock.Now, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                });
        }

        public async Task<ThrottleResult> ThrottleAsync(string filterKey, int maxRequests, int throttleDuration, int blockDuration)
        {
            if (await IsBlacklistAsync(filterKey))
            {
                return ThrottleResult.Blocked;
            }

            var throttleKey = GenerateThrottleKey(filterKey);
            var throttleCounter = await cacheService.GetAsync<ThrottleCounter>(throttleKey);
            throttleCounter = throttleCounter == null || clock.Now > throttleCounter.Timestamp + TimeSpan.FromSeconds(throttleDuration)
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

            if (throttleCounter.TotalRequests > maxRequests)
            {
                await AddBlacklistAsync(filterKey, blockDuration);
                return ThrottleResult.Blocked;
            }

            await cacheService.SetAsync(
                throttleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(throttleDuration)
                });
            return ThrottleResult.Allowed;
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var filterIdentity = filter.GetIdentity(httpContext);
            if (string.IsNullOrEmpty(filterIdentity))
            {
                return ThrottleResult.Allowed;
            }

            var filterKey = $"{policy.Name}_{filterIdentity}";
            return await ThrottleAsync(filterKey, policy.MaxRequests, policy.ThrottleDuration, policy.BlockDuration);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            IFilter filter)
        {
            var filterIdentity = filter.GetIdentity(HttpContext.Current);
            return await ThrottleAsync(work, blocked, policy, filterIdentity);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            IThrottlePolicy policy,
            string filterIdentity)
        {
            var filterKey = $"{policy.Name}_{filterIdentity}";
            if (await IsBlacklistAsync(filterKey))
            {
                return blocked();
            }

            async Task<TResult> success(TResult returnType)
            {
                var throttleResult = await ThrottleAsync(filterKey, policy.MaxRequests, policy.ThrottleDuration, policy.BlockDuration);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    await AddBlacklistAsync(filterKey, policy.BlockDuration);
                    return blocked();
                }

                return returnType;
            }

            return await work(success);
        }
    }
}
