namespace Alpha.NetThrottling.Utils
{
    using System.Net;
    using System.Web;

    public static class Extensions
    {
        public static IPAddress GetClientIp(this HttpRequestBase request)
        {
            return GetIPAddress(request.UserHostAddress);
        }

        public static IPAddress GetClientIp(this HttpRequest request)
        {
            return GetClientIp(new HttpRequestWrapper(request));
        }

        private static IPAddress GetIPAddress(string hostAddress)
        {
            var request = HttpContext.Current.Request;
            var actualIP = request.Headers["X-Actual-IP"];

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["HTTP_X_FORWARDED_FOR"] : actualIP;

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
               hostAddress : actualIP;

            actualIP = string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
                "127.0.0.1" : actualIP;

            string[] ipAddress = actualIP.Split(',');
            string[] ipparts = ipAddress[0].Split(':');
            if (IPAddress.TryParse(ipparts[0], out IPAddress ipFromHeader))
            {
                return ipFromHeader.MapToIPv4();
            }
            else
            {
                return IPAddress.Loopback.MapToIPv4();
            }
        }

        public static bool IsMatchRequest(this HttpRequest request, NetThrottlingKeyElement key)
        {
            return IsMatchRequest(new HttpRequestWrapper(request), key);
        }

        public static bool IsMatchRequest(this HttpRequestBase request, NetThrottlingKeyElement key)
        {
            var isPathMatched = key.Path == "*" || key.Path == request.AppRelativeCurrentExecutionFilePath;
            var isMethodMatched = key.Method == "*" || key.Method == request.HttpMethod;
            return isPathMatched && isMethodMatched;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return IsAjaxRequest(new HttpRequestWrapper(request));
        }

        public static bool IsAjaxRequest(this HttpRequestBase request)
        {
            if (request["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }
    }
}
