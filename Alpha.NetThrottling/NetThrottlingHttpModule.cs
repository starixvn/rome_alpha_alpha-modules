namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Web;

    public class NetThrottlingHttpModule : IHttpModule
    {
        private readonly INetThrottler netThrottler;
        private readonly IThrottleSetting throttleSetting;

        public NetThrottlingHttpModule()
            : this(NetThrottlingConfiguration.Default, null, null, null)
        {
        }

        public NetThrottlingHttpModule(string sectionName)
            : this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration)
            : this(configuration, null, null, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, ICacheService cacheService)
            : this(configuration, cacheService, null, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, ICacheService cacheService, IClock clock, ILogger logger)
        {
            throttleSetting = new ThrottleSetting(configuration);
            cacheService = cacheService ?? throttleSetting.CacheService;
            clock = clock ?? new Clock();
            netThrottler = new NetThrottler(throttleSetting, cacheService, clock, logger);
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += ThrottleRequest;
        }

        private void ThrottleRequest(object sender, EventArgs e)
        {
            var httpContext = ((HttpApplication)sender).Context;
            var httpContextBase = new HttpContextWrapper(httpContext);
            var throttleResult = Throttle(httpContextBase);
            if (throttleResult == ThrottleResult.Blocked)
            {
                throttleSetting.Resolver.Block(httpContextBase);
            }
        }

        private ThrottleResult Throttle(HttpContextBase httpContext)
        {
            foreach (var key in throttleSetting.Keys)
            {
                if (httpContext.Request.IsMatchRequest(key))
                {
                    var throttlePolicy = new ThrottlePolicy(key);
                    var filter = TypeFactory<IFilter>.Create(key.Filter);
                    return netThrottler.Throttle(httpContext, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }
    }
}
