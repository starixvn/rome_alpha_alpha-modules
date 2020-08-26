namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottlingHttpModule : IHttpModule
    {
        private readonly INetThrottler netThrottler;
        private readonly IThrottleSetting throttleSetting;

        public NetThrottlingHttpModule()
            : this(null, null, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, CacheService cacheService, Clock clock)
        {
            configuration = configuration ?? NetThrottlingConfiguration.Current;
            throttleSetting = new ThrottleSetting(configuration);
            cacheService = cacheService ?? CacheFactory.CreateDefaultService();
            clock = clock ?? new SystemClock();
            netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public void Init(HttpApplication context)
        {
            var wrapper = new EventHandlerTaskAsyncHelper(ThrottleRequest);
            context.AddOnBeginRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
            context.AddOnAcquireRequestStateAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        private async Task ThrottleRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var throttleResult = await ThrottleAsync(context);
            if (throttleResult == ThrottleResult.Blocked)
            {
                throttleSetting.Resolver.Block(context);
            }
        }

        private async Task<ThrottleResult> ThrottleAsync(HttpContext context)
        {
            foreach (var key in throttleSetting.Keys)
            {
                if (!key.Manual && key.IsMatchRequest(context.Request))
                {
                    var throttlePolicy = new ThrottlePolicy(key);
                    var filter = string.IsNullOrEmpty(key.Filter) ? null : TypeFactory<IFilter>.Create(key.Filter);
                    return await netThrottler.ThrottleAsync(context, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }
    }
}
