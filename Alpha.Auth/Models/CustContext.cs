namespace Alpha.Auth.Models
{
    using System;
    using System.Configuration;
    using System.Web;
    using Fanex.Common;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class CustContext : ICustContext
    {
        private const string CustomerClosedStatusSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingClosedStatus";
        private const string SubAccountPermissionSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingCustomerPermission";
        private const string StatusExpiredSessionKey = "aaaa";
        private const string UserPermissionSessionKey = "fffff";
        private IUserSession _userSession;

        public bool IsStatusActive
        {
            get
            {
                return Converter.ToBool(HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)]);
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)] = value;
            }
        }

        public string Permission
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)] = value;
            }
        }

        public DateTime StatusExpiredTime
        {
            private set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, StatusExpiredSessionKey)] = DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])).ToString();
            }

            get
            {
                return Converter.ToDateTime(HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, StatusExpiredSessionKey)], DateTime.Now.AddDays(-1));
            }
        }

        public DateTime PermissionExpiredTime
        {
            get
            {
                return Converter.ToDateTime(HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, UserPermissionSessionKey)], DateTime.Now.AddDays(-1));
            }

            private set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, UserPermissionSessionKey)] = DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])).ToString();
            }
        }

        public IUserSession UserSession
        {
            get
            {
                if (_userSession == null)
                {
                    _userSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
                }

                return _userSession;
            }
        }

        
    }
}