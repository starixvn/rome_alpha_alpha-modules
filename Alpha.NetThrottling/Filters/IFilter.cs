namespace Alpha.NetThrottling.Filters
{
    using System.Web;

    public interface IFilter
    {
        string GetKey(HttpContext httpContext);
    }
}
