namespace Alpha.NetThrottling.WebTest
{
    using Alpha.Logging;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggingConfig.Init();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 429)
            {
                return;
            }

            Debug.WriteLine(exception.Message);
        }
    }
}
