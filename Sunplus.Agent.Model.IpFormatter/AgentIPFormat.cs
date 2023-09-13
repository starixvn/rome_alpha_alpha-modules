namespace Sunplus.Agent.Model.Common
{
    using System;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Core.Common.IPs;

    /// <summary>
    /// IP Handle for Agent site
    /// </summary>
    [Obsolete("Should rename to Sunplus.Agent.Model.IpFormatter or delete it")]
    public class AgentIPFormat : IPFormat
    {
        public AgentIPFormat(string ip)
            : base(ip)
        {
        }

        protected override IContextIPInfo Context
        {
            get
            {
                IUserSession sessionInfo = UserSession.Current;
                return new ContextIPInfo
                {
                    AdminId = sessionInfo.AdminId,
                    IsAdminCanViewVNIP = sessionInfo.IsAdminCanViewVNIP
                };
            }
        }

        public static string FormatIP(string ip)
        {
            return new AgentIPFormat(ip).Format();
        }

        public static string FormatIPs(string content)
        {
            return new AgentIPFormat(string.Empty).FormatAll(content);
        }

        public static string[] GetClientIPs()
        {
            var request = HttpContext.Current.Request;
            var actualIP = request.Headers["X-Actual-IP"];

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["HTTP_X_FORWARDED_FOR"] : actualIP;

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["REMOTE_ADDR"] : actualIP;

            return string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
                new[] { "127.0.0.1" } : actualIP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetClientIP()
        {
            string[] ips = GetClientIPs();
            return ips[0].Trim();
        }
    }
}