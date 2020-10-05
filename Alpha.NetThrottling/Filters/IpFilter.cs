namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class IpFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext, IThrottlePolicy policy)
        {
            var request = httpContext.Request;
            var clientIp = request.GetClientIp().ToString();
            return CacheIdentity.From(policy, new string[] { request.Path, clientIp });
        }
    }
}
