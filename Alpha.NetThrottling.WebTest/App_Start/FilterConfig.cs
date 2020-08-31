using Alpha.NetThrottling.Mvc;
using System.Web.Mvc;

namespace Alpha.NetThrottling.WebTest
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NetThrottlingFilterAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
