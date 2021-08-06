namespace Alpha.SecurityCode.Web
{
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class SecurityCodeSessionImpl : SecurityCodeSession
    {
        private const string Hyphen = "_";

        //// security code
        private const string IsAlreadyInputSecurityCodeKey = "SecurityCodeSession.IsAlreadyInputSecurityCodeKey";
        private const string IsEnabledOtpKey = "SecurityCodeSession.IsEnabledOtp";

        public bool IsAlreadyInputSecurityCode
        {
            get
            {
                var isAlreadyInput = GetProperty(IsAlreadyInputSecurityCodeKey);
                return isAlreadyInput == null ? false : (bool)isAlreadyInput;
            }

            set
            {
                SetProperty(IsAlreadyInputSecurityCodeKey, value);
            }
        }

        public string SecurityCode
        {
            get
            {
                return UserSession.Current.SecurityCode;
            }
        }

        public bool IsForcedChangeSecurityCode
        {
            get
            {
                return UserSession.Current.IsForcedSecurityCode;
            }
        }

        public bool IsInternal
        {
            get
            {
                return UserSession.Current.IsInternal;
            }
        }

        public string UserSite
        {
            get
            {
                return UserSession.Current.Site;
            }
        }

        public bool IsEnabledOtp
        {
            get
            {
                var isEnabledOtp = GetProperty(IsEnabledOtpKey);
                return isEnabledOtp == null ? false : (bool)isEnabledOtp;
            }

            set
            {
                SetProperty(IsEnabledOtpKey, value);
            }
        }

        public bool IsEnabledSecurityCodeInBetLists
            => UserSession.Current.IsEnabledSecurityCodeInBetLists;

        public bool IsEnabledSecurityCodeInStatement
            => UserSession.Current.IsEnabledSecurityCodeInStatement;

        #region helper methods

        private static object GetProperty(string key)
        {
            return HttpContext.Current.Session[UserSession.Current.NetSessionId + Hyphen + key];
        }

        private static void SetProperty(string key, object value)
        {
            HttpContext.Current.Session[UserSession.Current.NetSessionId + Hyphen + key] = value;
        }

        #endregion helper methods
    }
}