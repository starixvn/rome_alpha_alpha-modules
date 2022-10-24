namespace Alpha.SecurityCode.Services
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Configurations;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class SecurityCodePathBuilderImpl : SecurityCodePathBuilder
    {
        public string BuildResetSecurityCodePath(string rootPath = null, string appVirtualPath = null)
        {
            return BuildRootPath(rootPath, appVirtualPath) + AppSettings.SecurityCodeConfig.ResetSecurityCodePath;
        }

        public string BuildVerifyingSecurityCodePath(
            string redirectUrl = "",
            IDictionary<string, string> queryStringItems = null,
            string rootPath = null,
            string appVirtualPath = null)
        {
            var queryString = string.Empty;
            if (queryStringItems != null
                && queryStringItems.Keys.Any())
            {
                queryString = "?";

                foreach (var queryStringItem in queryStringItems)
                {
                    queryString += $"{HttpUtility.UrlEncode(queryStringItem.Key)}={HttpUtility.UrlEncode(queryStringItem.Value)}&";
                }
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                if (AppSettings.SecurityCodeConfig.IsStandaloneSite)
                {
                    if (HttpContext.Current.Session.IsCookieless)
                    {
                        redirectUrl = $"{redirectUrl.Replace("/site-reports", string.Empty)}";
                    }
                    else
                    {
                        redirectUrl = $"/{(UserSession.Current.GetProperty<string>("Report_Routing") ?? string.Empty).TrimEnd('/')}{redirectUrl}";
                    }
                }

                queryString += (string.IsNullOrWhiteSpace(queryString) ? "?" : string.Empty)
                    + $"redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
            }

            var scRootPath = AppSettings.SecurityCodeConfig.IsStandaloneSite
                ? BuildRootPath()
                : BuildRootPath(rootPath, appVirtualPath);

            return $"{scRootPath}{BuildSecurityCheckingPath()}{queryString}";
        }

        private string BuildSecurityCheckingPath()
        {
            return SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp
                ? AppSettings.SecurityCodeConfig.VerifyingOtpPath
                : AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath;
        }

        private string BuildRootPath(string rootPath = null, string appVirtualPath = null)
        {
            rootPath = rootPath ?? HttpContext.Current.Response.ApplyAppPathModifier("~");

            if (string.IsNullOrWhiteSpace(AppSettings.SecurityCodeConfig.SiteMainApp))
            {
                if (!AppSettings.SecurityCodeConfig.IsSiteMain
                    && ConfigurationManager.AppSettings["MainAppPath"] != null)
                {
                    AppSettings.SecurityCodeConfig.SiteMainApp = ConfigurationManager.AppSettings["MainAppPath"];
                }
                else
                {
                    return rootPath;
                }
            }

            return rootPath.Replace(appVirtualPath ?? HttpRuntime.AppDomainAppVirtualPath, AppSettings.SecurityCodeConfig.SiteMainApp);
        }

        private string BuildRootPath()
        {
            AppSettings.SecurityCodeConfig.SiteMainApp = AppSettings.SecurityCodeConfig.SiteMainApp ?? ConfigurationManager.AppSettings["MainAppPath"];
            var cookieless = HttpContext.Current.Session.IsCookieless ? $"/(S({UserSession.Current.GetProperty<string>("MainSessionID")}))" : string.Empty;

            return $"{SecurityCodeSessionManager.SecurityCodeSession.MainDomain}{AppSettings.SecurityCodeConfig.SiteMainApp}{cookieless}/";
        }
    }
}