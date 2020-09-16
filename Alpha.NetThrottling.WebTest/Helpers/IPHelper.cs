namespace Alpha.NetThrottling.WebTest.Helpers
{
    using System.Web;

    public static class IPHelper
    {
        public static string ClientIP()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return (string.IsNullOrEmpty(ip) || ip.Equals("::1")) ? "127.0.0.1" : ip.Split(',')[0];
        }
    }
}