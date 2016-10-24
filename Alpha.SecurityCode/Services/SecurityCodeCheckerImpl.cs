namespace Alpha.SecurityCode.Services
{
    using System;
    using System.Linq;
    using Configurations;
    using Web;

    public class SecurityCodeCheckerImpl : SecurityCodeChecker
    {
        public bool IsRequiredSecurityCode(string relativePath)
        {
            if (ByPassSecurity())
            {
                return false;
            }

            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp 
                && !IsEnableSecurityCode())
            {
                return false;
            }

            return IsRequiredSecurity(relativePath);
        }

        private bool IsRequiredSecurity(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                var relativePathToLower = relativePath.ToLower();
                foreach (var url in AppSettings.SecurityCodeConfig.Paths)
                {
                    if (relativePathToLower.Contains(url.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ByPassSecurity()
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode
                || SecurityCodeSessionManager.SecurityCodeSession.IsInternal)
            {
                return true;
            }

            return false;
        }

        public bool IsEnableSecurityCode()
        {
            if (!AppSettings.SecurityCodeConfig.IsActive)
            {
                return false;
            }

            //// If forced change, then user have been offered security code
            if (IsForcedChangeSecurityCode())
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

            //// If user do not have security code, then user are not offered SC
            if (string.IsNullOrWhiteSpace(SecurityCodeSessionManager.SecurityCodeSession.SecurityCode))
            {
                return false;
            }

            return true;
        }

        public bool IsForcedChangeSecurityCode()
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsInternal
                && SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode)
            {
                return true;
            }

            return false;
        }

        private bool IsEnableOtp()
        {
            return SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp;
        }
    }
}