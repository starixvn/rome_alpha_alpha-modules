namespace Alpha.SecurityCode.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Configurations;
    using Web;

    public static class SecurityCodeUrlBuilder
    {
        public static string BuildResetSecurityCodeUrl()
        {
            return BuildRootUrl() + AppSettings.SecurityCodeConfig.ResetSecurityCodeUrl;
        }

        public static string BuildChangeSecurityCodeUrl()
        {
            return BuildRootUrl() + AppSettings.SecurityCodeConfig.ChangeSecurityCodeUrl;
        }

        public static string BuildVerifyingSecurityCodeUrl(string redirectUrl = "", IDictionary<string,string> queryStringItems = null)
        {
            var queryString = string.Empty;
            if(queryStringItems != null 
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
                queryString += (string.IsNullOrWhiteSpace(queryString) ? "?" : queryString) 
                    +  $"redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
            }

            return BuildRootUrl() + AppSettings.SecurityCodeConfig.VerifyingSecurityCodeUrl + queryString;
        }

        private static string BuildRootUrl()
        {
            var rootUrl = HttpContext.Current.Response.ApplyAppPathModifier("~");

            return Regex.Replace(rootUrl, AppSettings.SecurityCodeConfig.CurrentSiteApp, AppSettings.SecurityCodeConfig.SiteMainApp, RegexOptions.IgnoreCase);
        }
    }
}
