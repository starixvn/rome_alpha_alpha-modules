namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class NetThrottlingFilterAttribute : ActionFilterAttribute
    {
        private readonly NetThrottlingConfiguration configuration;
        private readonly ThrottlePolicy policy;
        private readonly CacheService cache;
        private readonly Clock clock;
        private readonly IResolver resolver;

        public NetThrottlingFilterAttribute(string keyName)
            : this(keyName, null, null, null)
        {
        }

        public NetThrottlingFilterAttribute(string keyName, NetThrottlingConfiguration configuration, CacheService cache, Clock clock)
        {
            this.configuration = configuration ?? NetThrottlingConfiguration.Current;
            this.cache = cache ?? CacheFactory.CreateDefaultService();
            this.clock = clock ?? new SystemClock();
            resolver = string.IsNullOrEmpty(this.configuration.Resolver) ? new DefaultResolver() : TypeFactory<IResolver>.Create(this.configuration.Resolver);
            var key = this.configuration.Keys[keyName];
            if (!key.Manual)
            {
                policy = Mapper.From(key);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (policy == null)
            {
                return;
            }

            var httpContext = HttpContext.Current;
            var netThrotter = new NetThrottler(configuration, cache, clock);
            var applyingPolicy = GetApplyingPolicy(filterContext);
            if (applyingPolicy != null)
            {
                var throttleResult = netThrotter.ThrottleAsync(httpContext, applyingPolicy).Result;
                if (throttleResult == ThrottleResult.Blocked)
                {
                    resolver.Block(httpContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private ThrottlePolicy GetApplyingPolicy(ActionExecutingContext filterContext)
        {
            ThrottlePolicy applyingPolicy = policy;

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(EnableNetThrottlingAttribute), true))
            {
                var attr = (EnableNetThrottlingAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(EnableNetThrottlingAttribute), true).First();
                applyingPolicy = attr.Policy;
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DisableNetThrottlingAttribute), true))
            {
                applyingPolicy = null;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(EnableNetThrottlingAttribute), true))
            {
                var attr = (EnableNetThrottlingAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(EnableNetThrottlingAttribute), true).First();
                applyingPolicy = attr.Policy;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableNetThrottlingAttribute), true))
            {
                applyingPolicy = null;
            }

            return applyingPolicy;
        }
    }
}
