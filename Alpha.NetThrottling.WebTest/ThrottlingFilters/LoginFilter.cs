namespace Alpha.NetThrottling.WebTest.ThrottlingFilters
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class LoginFilter : IFilter
    {
        public string GetIdentity(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var clientIp = request.GetClientIp().ToString();

            var username = request.Form["txtUserName"];
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            username = username.Trim().ToLowerInvariant();
            return $"{clientIp}_{username}";
        }
    }
}