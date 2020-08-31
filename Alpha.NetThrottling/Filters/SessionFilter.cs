namespace Alpha.NetThrottling.Filters
{
    using System.Web;

    public class SessionFilter : IFilter
    {
        public string GetIdentity(HttpContextBase httpContext)
        {
            return $"{httpContext.Request.Path}_{httpContext.Session.SessionID}";
        }
    }
}
