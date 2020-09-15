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
            : this(NetThrottlingConfiguration.Default, null, null)
        {
        }

        public NetThrottlingHttpModule(string sectionName)
            : this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration)
            : this(configuration, null, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, ICacheService cacheService)
            : this(configuration, cacheService, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, ICacheService cacheService, IClock clock)
        {
            throttleSetting = new ThrottleSetting(configuration);
            cacheService = cacheService ?? throttleSetting.CacheService;
            clock = clock ?? new Clock();
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
        }

        private async Task ThrottleRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var contextBase = new HttpContextWrapper(context);
            var throttleResult = await ThrottleAsync(contextBase);
            if (throttleResult == ThrottleResult.Blocked)
            {
                throttleSetting.Resolver.Block(contextBase);
            }
        }

        private async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext)
        {
            foreach (var key in throttleSetting.Keys)
            {
                if (httpContext.Request.IsMatchRequest(key))
                {
                    var throttlePolicy = new ThrottlePolicy(key);
                    var filter = TypeFactory<IFilter>.Create(key.Filter);
                    return await netThrottler.ThrottleAsync(httpContext, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }
    }
}
