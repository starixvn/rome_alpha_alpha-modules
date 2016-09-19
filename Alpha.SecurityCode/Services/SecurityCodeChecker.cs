namespace Alpha.SecurityCode.Services
{
    using System;
    using System.Linq;
    using Configurations;
    using Web;

    public static class SecurityCodeChecker
    {
        public static bool IsRequiredSecurityCode(string relativeUrl)
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode)
            {
                return false;
            }

            if(!IsEnableSecurityCode())
            {
                return false;
            }

            var relativeUrlToLower = relativeUrl.ToLower();
            foreach (var url in AppSettings.SecurityCodeConfig.Urls)
            {
                if (relativeUrlToLower.Contains(url.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsEnableSecurityCode()
        {
            if(!AppSettings.SecurityCodeConfig.IsActive)
            {
                return false;
            }
            
            if(IsForcedChangeSecurityCode())
            {
                return true;
            }

            //// User is not belong to site needed check security code
            if (AppSettings.SecurityCodeConfig.Sites.Any()
                && !string.IsNullOrWhiteSpace(SecurityCodeSessionManager.SecurityCodeSession.UserSite)
                && !AppSettings.SecurityCodeConfig.Sites.Contains(SecurityCodeSessionManager.SecurityCodeSession.UserSite, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(SecurityCodeSessionManager.SecurityCodeSession.SecurityCode))
            {
                return false;
            }

            return true;
        }

        public static bool IsForcedChangeSecurityCode()
        {
            //// If forced change, then user have been offered security code
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsInternal
                && SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode)
            {
                return true;
            }

            return false;
        }
    }
}