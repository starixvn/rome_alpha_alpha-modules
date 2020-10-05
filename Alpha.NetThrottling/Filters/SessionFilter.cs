namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using System.Web;

    public class SessionFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext, IThrottlePolicy policy)
        {
            return CacheIdentity.From(policy, new string[] { httpContext.Request.Path, httpContext.Session.SessionID });
        }
    }
}
