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
            return GetIPAddress(request.Headers["X-Forwarded-For"], request.UserHostAddress);
        }

        private static IPAddress GetIPAddress(string header, string hostaddress)
        {
            if (string.IsNullOrEmpty(header))
            {
                return IPAddress.Parse(hostaddress).MapToIPv4();
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
    }
}
