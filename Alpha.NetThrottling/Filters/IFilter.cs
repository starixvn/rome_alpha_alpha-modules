namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Models;
    using System.Web;

    public interface IFilter
    {
        CacheIdentity GetCacheIdentity(HttpContextBase httpContext);
    }
}
