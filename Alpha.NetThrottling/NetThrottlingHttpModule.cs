namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System;
    using System.Web;

    public class NetThrottlingHttpModule : IHttpModule
    {
        private readonly INetThrottler netThrottler;

        public NetThrottlingHttpModule()
            : this(null)
        {
        }

        public NetThrottlingHttpModule(INetThrottler netThrottler)
        {
            this.netThrottler = netThrottler ?? new NetThrottler();
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
                netThrottler.ThrottleSetting.Resolver.Block(httpContextBase);
            }
        }

        private ThrottleResult Throttle(HttpContextBase httpContext)
        {
            foreach (var key in netThrottler.ThrottleSetting.Keys)
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
