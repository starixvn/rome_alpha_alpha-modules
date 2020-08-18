namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottlingHttpModule : IHttpModule
    {
        private readonly NetThrottler netThrottler;
        private readonly IResolver resolver;

        public NetThrottlingHttpModule()
            :this(null, null, null)
        {
        }

        public NetThrottlingHttpModule(NetThrottlingConfiguration configuration, CacheService cache, Clock clock)
        {
            configuration = configuration ?? NetThrottlingConfiguration.Current;
            cache = cache ?? CacheFactory.CreateDefaultService();
            clock = clock ?? new SystemClock();
            netThrottler = new NetThrottler(configuration, cache, clock);
            resolver = string.IsNullOrEmpty(configuration.Resolver) ? new DefaultResolver() : TypeFactory<IResolver>.Create(configuration.Resolver);
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

        async Task ThrottleRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var throttleResult = await netThrottler.ThrottleAsync(context);
            if (throttleResult == ThrottleResult.Blocked)
            {
                resolver.Block(context);
            }
        }
    }
}
