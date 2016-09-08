namespace Alpha.SecurityCode.Web
{
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class SecurityCodeSession : ISecurityCodeSession
    {
        private const string Hyphen = "_";

        //// security code
        private const string IsAlreadyInputSecurityCodeKey = "SecurityCodeSession.IsAlreadyInputSecurityCodeKey";

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