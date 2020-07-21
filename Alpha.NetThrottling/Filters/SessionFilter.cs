namespace Alpha.NetThrottling.Filters
{
    using System.Web;

    public class SessionFilter : IFilter
    {
        public string GetKey(HttpContext httpContext)
        {
            return $"{httpContext.Session.SessionID}";
        }
    }
}
