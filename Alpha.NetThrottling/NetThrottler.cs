namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Fanex.Caching;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottler
    {
        public NetThrottlingConfiguration Configuration { get; private set; }

        public CacheService Cache { get; private set; }

        public NetThrottler()
            : this(null, null)
        {
        }

        public NetThrottler(NetThrottlingConfiguration configuration, CacheService cache)
        {
            Configuration = configuration ?? NetThrottlingConfiguration.Current;
            Cache = cache ?? new CacheServiceImpl();
        }

        public async Task<bool> ThrottleAsync(HttpContext context)
        {
            var request = context.Request;
            foreach (NetThrottlingKeyElement key in Configuration.Keys)
            {
                var isPathMatched = key.Path == "*" || key.Path == request.AppRelativeCurrentExecutionFilePath;
                var isMethodMatched = key.Method == "*" || key.Method == request.HttpMethod;
                if (isPathMatched && isMethodMatched)
                {
                    return await ThrottleAsync(context, key);
                }
            }

            return true;
        }

        private async Task<bool> ThrottleAsync(HttpContext httpContext, NetThrottlingKeyElement key)
        {
            var filter = FilterFactory.Create(key.Filter);
            var filterKey = filter.GetKey(httpContext);
            if (string.IsNullOrEmpty(filterKey))
            {
                return true;
            }

            var id = $"{Configuration.Prefix}_{key.Name}_{filterKey}";
            var throttleCounter = await Cache.GetAsync<ThrottleCounter>(id);
            throttleCounter = throttleCounter == null || DateTime.Now > throttleCounter.Timestamp + TimeSpan.FromSeconds(key.PeriodInSeconds)
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

            if (throttleCounter.TotalRequests > key.MaxRequests)
            {
                return false;
            }

            await Cache.SetAsync(
                id,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(key.PeriodInSeconds)
                });
            return true;
        }
    }
}
