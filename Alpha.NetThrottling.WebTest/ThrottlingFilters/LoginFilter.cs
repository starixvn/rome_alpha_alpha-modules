namespace Alpha.NetThrottling.WebTest.ThrottlingFilters
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class LoginFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContextBase httpContext, IThrottlePolicy policy)
        {
            var request = httpContext.Request;
            var clientIp = request.GetClientIp().ToString();

            var username = request.Form["Username"];
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            username = username.Trim().ToLowerInvariant();
            return CacheIdentity.From(clientIp, username, policy);
        }
    }
}