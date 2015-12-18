namespace Fanex.Razorbill.Security.Services
{
    using System.Web;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;

    /// <summary>
    /// SecurityCodeChecker class.
    /// </summary>
    public static class SecurityCodeChecker
    {
        /// <summary>
        /// Gets the redirect URL.
        /// </summary>
        /// <returns>The redirect URL.</returns>
        public static string GetRedirectUrl()
        {
            if (!SecurityCodeConfig.IsActive || HttpContext.Current.Session == null)
            {
                return null;
            }

            SecurityCodeStatus status = (SecurityCodeStatus)CookieLessSessionFactory.SecurityCodeSession.SecurityCodeResult;
            if (status == SecurityCodeStatus.NONE || status == SecurityCodeStatus.PROVIDED_SC_SUCCESS)
            {
                return null;
            }

            // Check ignore Urls
            HttpRequest currentRequest = System.Web.HttpContext.Current.Request;
            string urlWithoutParams = currentRequest.Url.AbsolutePath.ToLower();
            if (SecurityCodeConfig.IsIgnoreSecurityCode(urlWithoutParams))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(currentRequest.Url.Query))
            {
                string fullUrl = currentRequest.RawUrl.ToLower();
                if (SecurityCodeConfig.IsIgnoreSecurityCode(fullUrl))
                {
                    return null;
                }
            }

            string redirectUrl = null;
            switch (status)
            {
                case SecurityCodeStatus.PROVIDED_SC:
                    redirectUrl = "~/Security/SecurityCode/VerifySecurityCode";
                    break;

                case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
                    redirectUrl = "~/Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation";
                    break;

                case SecurityCodeStatus.FORCED_CHANGE_SC:
                    redirectUrl = "~/Security/SecurityCode/ForceChangeSecurityCode";
                    break;

                default:
                    break;
            }

            return redirectUrl;
        }
    }
}
