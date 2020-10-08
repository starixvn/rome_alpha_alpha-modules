namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class NetThrottlingFilterAttribute : ActionFilterAttribute
    {
        private readonly IThrottleSetting throttleSetting;
        private readonly INetThrottler netThrottler;

        public NetThrottlingFilterAttribute()
            : this(NetThrottlingConfiguration.Default)
        {
        }

        public NetThrottlingFilterAttribute(string sectionName)
            : this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration)
            : this(configuration, null, null, null)
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration, ICacheService cacheService)
            : this(configuration, cacheService, null, null)
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration, ICacheService cacheService, IClock clock, ILogger logger)
        {
            throttleSetting = new ThrottleSetting(configuration);
            cacheService = cacheService ?? throttleSetting.CacheService;
            clock = clock ?? new Clock();
            netThrottler = new NetThrottler(throttleSetting, cacheService, clock, logger);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var throttleResult = Throttle(filterContext);
            if (throttleResult == ThrottleResult.Blocked)
            {
                throttleSetting.Resolver.Block(filterContext.HttpContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private ThrottleResult Throttle(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            foreach (var key in throttleSetting.Keys)
            {
                if (httpContext.Request.IsMatchRequest(key))
                {
                    var throttlePolicy = GetThrottlePolicy(filterContext, key);
                    if (throttlePolicy == null)
                    {
                        return ThrottleResult.Allowed;
                    }

                    var filter = TypeFactory<IFilter>.Create(key.Filter);
                    return netThrottler.Throttle(httpContext, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }

        private IThrottlePolicy GetThrottlePolicy(ActionExecutingContext filterContext, NetThrottlingKeyElement key)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableNetThrottlingAttribute), true))
            {
                return null;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(EnableNetThrottlingAttribute), true))
            {
                var attr = (EnableNetThrottlingAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(EnableNetThrottlingAttribute), true).First();
                return attr.Policy;
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DisableNetThrottlingAttribute), true))
            {
                return null;
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(EnableNetThrottlingAttribute), true))
            {
                var attr = (EnableNetThrottlingAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(EnableNetThrottlingAttribute), true).First();
                return attr.Policy;
            }

            return new ThrottlePolicy(key);
        }
    }
}
