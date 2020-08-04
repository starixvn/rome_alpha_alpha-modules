namespace Alpha.NetThrottling.Filters
{
    using System.Web;

    public interface IFilter
    {
        string GetIdentity(HttpContext httpContext);
    }
}
