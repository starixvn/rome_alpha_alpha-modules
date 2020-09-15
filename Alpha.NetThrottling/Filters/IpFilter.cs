namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class IpFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var clientIp = request.GetClientIp().ToString();
            return CacheIdentity.PathAndKey(request.Path, clientIp);
        }
    }
}
