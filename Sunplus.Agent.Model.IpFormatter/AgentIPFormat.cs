namespace Sunplus.Agent.Model.Common
{
    using System;
    using System.Web;
    using Fanex.Logging.Extensions.Web;
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
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrWhiteSpace(ip))
            {
                ExceptionHandler.Handle(new System.Exception("After login: ip is null or empty"), System.Diagnostics.TraceEventType.Error);
            }

            var ips = string.IsNullOrWhiteSpace(ip) || ip.Equals("::1") ?
                new[] { "127.0.0.1" } : ip.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrWhiteSpace(ips[0]))
            {
                ExceptionHandler.Handle(new System.Exception("After login: HTTP_X_FORWARDED_FOR ip is null or empty"), System.Diagnostics.TraceEventType.Error);
            }

            return ips;
        }

        public static string GetClientIP()
        {
            string[] ips = GetClientIPs();
            return ips[0].Trim();
        }
    }
}