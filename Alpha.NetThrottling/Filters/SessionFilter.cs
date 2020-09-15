namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using System.Web;

    public class SessionFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext, IThrottlePolicy policy)
        {
            return CacheIdentity.From(httpContext.Request.Path, httpContext.Session.SessionID, policy);
        }
    }
}
