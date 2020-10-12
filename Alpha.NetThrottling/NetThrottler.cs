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
        private readonly IClock clock;
        private readonly ILogger logger;

        public IThrottleSetting ThrottleSetting { get; private set; }

        public ICacheService CacheService
        {
            get
            {
                return ThrottleSetting.CacheService;
            }
        }

        public NetThrottler()
            : this(null, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting)
            : this(throttleSetting, null, null)
        {
        }

        public NetThrottler(IThrottleSetting throttleSetting, IClock clock, ILogger logger)
        {
            ThrottleSetting = throttleSetting ?? new ThrottleSetting();
            this.clock = clock ?? new Clock();
            this.logger = logger ?? Logger.Default;
        }

        private bool IsBlacklist(string blockKey)
        {
            return CacheService.Contain(blockKey);
        }

        private void AddBlacklist(string blockKey, int blockDuration)
        {
            CacheService.Set(
                blockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                });
        }

        public ThrottleResult Throttle(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return Throttle(policy, cacheIdentity);
        }

        public ThrottleResult Throttle(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            try
            {
                if (IsBlacklist(cacheIdentity.BlockKey))
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

                if (throttleCounter.TotalRequests > policy.MaxRequests)
                {
                    AddBlacklist(cacheIdentity.BlockKey, policy.BlockDuration);
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
            catch (Exception ex)
            {
                logger.Error(ex);
                return ThrottleResult.Allowed;
            }
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
                    try
                    {
                        AddBlacklist(cacheIdentity.BlockKey, policy.BlockDuration);
                        return blocked();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        return returnType;
                    }
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return work(success);
            }

            try
            {
                if (IsBlacklist(cacheIdentity.BlockKey))
                {
                    return blocked();
                }

                return work(success);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return work(success);
            }
        }

        private async Task<bool> IsBlacklistAsync(string blockKey)
        {
            return await CacheService.ContainAsync(blockKey).ConfigureAwait(false);
        }

        private async Task AddBlacklistAsync(string blockKey, int blockDuration)
        {
            await CacheService.SetAsync(
                blockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(blockDuration)
                }).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext, IThrottlePolicy policy, IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(IThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            try
            {
                if (await IsBlacklistAsync(cacheIdentity.BlockKey).ConfigureAwait(false))
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

                if (throttleCounter.TotalRequests > policy.MaxRequests)
                {
                    await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration).ConfigureAwait(false);
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
            catch (Exception ex)
            {
                logger.Error(ex);
                return ThrottleResult.Allowed;
            }
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
                    try
                    {
                        await AddBlacklistAsync(cacheIdentity.BlockKey, policy.BlockDuration).ConfigureAwait(false);
                        return blocked();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        return returnType;
                    }
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return await work(success).ConfigureAwait(false);
            }

            try
            {
                if (await IsBlacklistAsync(cacheIdentity.BlockKey).ConfigureAwait(false))
                {
                    return blocked();
                }

                return await work(success).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return await work(success).ConfigureAwait(false);
            }
        }
    }
}
