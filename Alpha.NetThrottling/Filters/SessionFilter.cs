namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using System.Web;

    public class SessionFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext)
        {
            return CacheIdentity.PathAndKey(httpContext.Request.Path, httpContext.Session.SessionID);
        }
    }
}
