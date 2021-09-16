namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class SessionFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext, IThrottlePolicy policy)
        {
            var userSession = UserSessionFactory.Create();
            return CacheIdentity.From(policy, new [] { httpContext.Request.Path, userSession.UserName, userSession.SubUserName });
        }
    }
}
