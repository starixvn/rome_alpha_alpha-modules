namespace Alpha.NetThrottling.Filters
{
    using System.Web;

    public class SessionFilter : IFilter
    {
        public string GetIdentity(HttpContext httpContext)
        {
            return $"{httpContext.Request.Path}_{httpContext.Session.SessionID}";
        }
    }
}
