namespace Alpha.NetThrottling.Utils
{
    using System.Net;
    using System.Web;

    public static class Extensions
    {
        public static IPAddress GetClientIp(this HttpRequestBase request)
        {
            return GetIPAddress(request.Headers["X-Forwarded-For"], request.UserHostAddress);
        }

        public static IPAddress GetClientIp(this HttpRequest request)
        {
            return GetClientIp(new HttpRequestWrapper(request));
        }

        private static IPAddress GetIPAddress(string header, string hostAddress)
        {
            if (string.IsNullOrEmpty(header))
            {
                return IPAddress.Parse(hostAddress).MapToIPv4();
            }
            else
            {
                string[] ipAddress = header.Split(',');
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
