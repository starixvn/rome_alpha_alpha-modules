namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class NetThrottlingFilterAttribute : ActionFilterAttribute
    {
        private readonly IThrottleSetting throttleSetting;
        private readonly INetThrottler netThrottler;

        public NetThrottlingFilterAttribute()
            :this(NetThrottlingConfiguration.Default)
        {
        }

        public NetThrottlingFilterAttribute(string sectionName)
            : this(NetThrottlingConfiguration.Get(sectionName))
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration)
            : this(configuration, null, null)
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration, CacheService cacheService)
            : this(configuration, cacheService, null)
        {
        }

        public NetThrottlingFilterAttribute(NetThrottlingConfiguration configuration, CacheService cacheService, Clock clock)
        {
            throttleSetting = new ThrottleSetting(configuration);
            cacheService = cacheService ?? CacheFactory.CreateDefaultService();
            clock = clock ?? new SystemClock();
            netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = HttpContext.Current;
            var throttleResult = ThrottleAsync(filterContext, httpContext).Result;
            if (throttleResult == ThrottleResult.Blocked)
            {
                throttleSetting.Resolver.Block(httpContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private async Task<ThrottleResult> ThrottleAsync(ActionExecutingContext filterContext, HttpContext httpContext)
        {
            foreach (var key in throttleSetting.Keys)
            {
                if (key.IsMatchRequest(httpContext.Request))
                {
                    var throttlePolicy = GetThrottlePolicy(filterContext, key);
                    if(throttlePolicy == null)
                    {
                        return ThrottleResult.Allowed;
                    }

                    var filter = TypeFactory<IFilter>.Create(key.Filter);
                    return await netThrottler.ThrottleAsync(httpContext, throttlePolicy, filter);
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
