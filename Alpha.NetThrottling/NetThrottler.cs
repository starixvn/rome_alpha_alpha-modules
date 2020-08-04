namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottler
    {
        private const string BLOCK_PREFIX = "Block";
        private const string THROTTLE_PREFIX = "Throttle";

        private readonly NetThrottlingConfiguration configuration;

        private readonly CacheService cache;

        public NetThrottler()
            : this(null, null)
        {
        }

        public NetThrottler(NetThrottlingConfiguration configuration, CacheService cache)
        {
            this.configuration = configuration ?? NetThrottlingConfiguration.Current;
            this.cache = cache ?? CacheFactory.CreateDefaultService();
        }

        private string GenerateThrottleKey(string filterKey)
        {
            return $"{configuration.Prefix}_{THROTTLE_PREFIX}_{filterKey}";
        }

        private string GenerateBlockKey(string filterKey)
        {
            return $"{configuration.Prefix}_{BLOCK_PREFIX}_{filterKey}";
        }

        private async Task<bool> IsBlacklistAsync(string filterKey)
        {
            var blockKey = GenerateBlockKey(filterKey);
            var blockValue = await cache.GetAsync<DateTime>(blockKey);
            if (blockValue != DateTime.MinValue)
            {
                return true;
            }

            return false;
        }

        private async Task AddBlacklistAsync(string filterKey, int blockDuration)
        {
            var blockKey = GenerateBlockKey(filterKey);
            await cache.SetAsync(
                blockKey,
                DateTime.Now, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                });
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContext context)
        {
            var request = context.Request;
            foreach (NetThrottlingKeyElement key in configuration.Keys)
            {
                if (key.Manual)
                {
                    continue;
                }

                var isPathMatched = key.Path == "*" || key.Path == request.AppRelativeCurrentExecutionFilePath;
                var isMethodMatched = key.Method == "*" || key.Method == request.HttpMethod;
                if (isPathMatched && isMethodMatched)
                {
                    return await ThrottleAsync(context, Mapper.From(key));
                }
            }

            return ThrottleResult.Allowed;
        }

        public async Task<ThrottleResult> ThrottleAsync(string filterKey, int maxRequests, int throttleDuration, int blockDuration)
        {
            if (blockDuration != 0 && await IsBlacklistAsync(filterKey))
            {
                return ThrottleResult.Blocked;
            }

            var throttleKey = GenerateThrottleKey(filterKey);
            var throttleCounter = await cache.GetAsync<ThrottleCounter>(throttleKey);
            throttleCounter = throttleCounter == null || DateTime.Now > throttleCounter.Timestamp + TimeSpan.FromSeconds(throttleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = DateTime.Now,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > maxRequests)
            {
                if (blockDuration != 0)
                {
                    await AddBlacklistAsync(filterKey, blockDuration);
                }

                return ThrottleResult.Blocked;
            }

            await cache.SetAsync(
                throttleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(throttleDuration)
                });
            return ThrottleResult.Allowed;
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, ThrottlePolicy policy)
        {
            var filter = TypeFactory<IFilter>.Create(policy.Filter);
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
            string filterIdentity,
            ThrottlePolicy policy)
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
