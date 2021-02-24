namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottlingAsyncHttpModule : IHttpModule
    {
        private readonly INetThrottler netThrottler;

        public NetThrottlingAsyncHttpModule()
            : this(null)
        {
        }

        public NetThrottlingAsyncHttpModule(INetThrottler netThrottler)
        {
            this.netThrottler = netThrottler ?? new NetThrottler();
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public void Init(HttpApplication context)
        {
            var wrapper = new EventHandlerTaskAsyncHelper(ThrottleRequestAsync);
            context.AddOnBeginRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        private async Task ThrottleRequestAsync(object sender, EventArgs e)
        {
            var httpContext = ((HttpApplication)sender).Context;
            var httpContextBase = new HttpContextWrapper(httpContext);
            var throttleResult = await ThrottleAsync(httpContextBase);
            if (throttleResult == ThrottleResult.Blocked)
            {
                netThrottler.ThrottleSetting.Resolver.Block(httpContextBase);
            }
        }

        private async Task<ThrottleResult> ThrottleAsync(HttpContextBase httpContext)
        {
            foreach (var key in netThrottler.ThrottleSetting.Keys)
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
