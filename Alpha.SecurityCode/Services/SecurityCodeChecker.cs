namespace Alpha.SecurityCode.Services
{
    using System.Web;
    using Alpha.SecurityCode.Configurations;
    using Alpha.SecurityCode.Entities;
    using Alpha.SecurityCode.Sessions;

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
                    redirectUrl = SecurityCodeSection.Instance.SecurityCodeVerifyUrl;
                    break;

                case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
                    redirectUrl = SecurityCodeSection.Instance.ForceChangeSecurityCodeUrl;
                    break;

                case SecurityCodeStatus.FORCED_CHANGE_SC:
                    redirectUrl = SecurityCodeSection.Instance.ForceChangeSecurityCodeWithoutValidationUrl;
                    break;

                default:
                    break;
            }

            return redirectUrl;
        }
    }
}