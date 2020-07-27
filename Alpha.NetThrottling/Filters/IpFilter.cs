namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class IpFilter : IFilter
    {
        public string GetIdentity(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var clientIp = request.GetClientIp().ToString();
            return $"{clientIp}";
        }
    }
}
