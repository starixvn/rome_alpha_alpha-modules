namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class NetThrottlingFilterAttribute : ActionFilterAttribute
    {
        private readonly NetThrottlingKeyElement key;
        private readonly IThrottleSetting throttleSetting;
        private readonly CacheService cacheService;
        private readonly Clock clock;

        public NetThrottlingFilterAttribute(string keyName)
            : this(keyName, null, null, null)
        {
        }

        public NetThrottlingFilterAttribute(string keyName, NetThrottlingConfiguration configuration, CacheService cacheService, Clock clock)
        {
            configuration = configuration ?? NetThrottlingConfiguration.Current;
            throttleSetting = new ThrottleSetting(configuration);
            key = throttleSetting.GetKey(keyName);
            
            this.cacheService = cacheService ?? CacheFactory.CreateDefaultService();
            this.clock = clock ?? new SystemClock();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (key.Manual)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var filter = string.IsNullOrEmpty(key.Filter) ? null : TypeFactory<IFilter>.Create(key.Filter);
            var httpContext = HttpContext.Current;
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var applyingPolicy = GetApplyingPolicy(filterContext);
            if (applyingPolicy != null)
            {
                var throttleResult = netThrottler.ThrottleAsync(httpContext, applyingPolicy, filter).Result;
                if (throttleResult == ThrottleResult.Blocked)
                {
                    throttleSetting.Resolver.Block(httpContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private IThrottlePolicy GetApplyingPolicy(ActionExecutingContext filterContext)
        {
            IThrottlePolicy applyingPolicy = new ThrottlePolicy(key);

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
