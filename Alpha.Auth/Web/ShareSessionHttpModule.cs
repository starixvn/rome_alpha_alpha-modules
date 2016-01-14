namespace Alpha.Auth.Web
{
    using System.Configuration;
    using System.Reflection;
    using System.Web;

    public static class ShareSession
    {
        public static void Init()
        {
            string applicationName = ConfigurationManager.AppSettings["ApplicationName"];

            applicationName = applicationName ?? "AlphaSite";

            // Change the Application Name in runtime.
            var runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
            var theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
            var appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);

            appNameInfo.SetValue(theRuntime, applicationName);
        }
    }
}