namespace Fanex.Razorbill.Security.Helpers
{
    using System;
    using System.Web;

    /// <summary>
    /// IP Helper.
    /// </summary>
    internal class IPHelper
    {
        public static string CustomerIp()
        {
            var request = HttpContext.Current.Request;
            var actualIP = request.Headers["X-Actual-IP"];

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["HTTP_X_FORWARDED_FOR"] : actualIP;

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["REMOTE_ADDR"] : actualIP;

            return string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
                "127.0.0.1" : actualIP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }
    }
}