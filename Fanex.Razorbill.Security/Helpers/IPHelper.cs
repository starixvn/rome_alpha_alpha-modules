namespace Fanex.Razorbill.Security.Helpers
{
    using System.Web;

    /// <summary>
    /// IP Helper.
    /// </summary>
    internal class IPHelper
    {
        public static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip.Equals("::1") ? "127.0.0.1" : (string.IsNullOrEmpty(ip) ? "127.0.0.1" : ip.Split(',')[0]);
        }
    }
}