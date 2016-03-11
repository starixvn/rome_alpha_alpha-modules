namespace Alpha.Auth.Models
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.SessionState;
    using Fanex.Common;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class CustContext : ICustContext
    {
        private const string CustomerClosedStatusSessionKey = "Alpha.Auth.CustContext.CustomerClosedStatusSessionKey";
        private const string SubAccountPermissionSessionKey = "Alpha.Auth.CustContext.SubAccountPermissionSessionKey";
        private const string StatusExpiredSessionKey = "Alpha.Auth.CustContext.StatusExpiredSessionKey";
        private const string UserPermissionSessionKey = "Alpha.Auth.CustContext.UserPermissionSessionKey";
        private const string SessionKeyFormat = "{0}_{1}";

        private readonly int _custStatusExpireTime;

        private IUserSession _userSession;
        private HttpSessionState _httpSession = HttpContext.Current.Session;

        public CustContext()
        {
            _custStatusExpireTime = Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"]);
        }

        public bool IsActive
        {
            get
            {
                return Converter.ToBool(_httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, CustomerClosedStatusSessionKey)]);
            }

            set
            {
                _httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, CustomerClosedStatusSessionKey)] = value;
            }
        }

        public string SubAccPermission
        {
            get
            {
                return (string)_httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, SubAccountPermissionSessionKey)];
            }

            set
            {
                _httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, SubAccountPermissionSessionKey)] = value;
            }
        }

        public DateTime StatusCachedExpiredTime
        {
            get
            {
                return Converter.ToDateTime(_httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, StatusExpiredSessionKey)], DateTime.Now.AddDays(-1));
            }

            private set
            {
                _httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, StatusExpiredSessionKey)] = DateTime.Now.AddSeconds(_custStatusExpireTime).ToString();
            }
        }

        public DateTime SubAccPermissionExpiredTime
        {
            get
            {
                return Converter.ToDateTime(_httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, UserPermissionSessionKey)], DateTime.Now.AddDays(-1));
            }

            private set
            {
                _httpSession[string.Format(SessionKeyFormat, _httpSession.SessionID, UserPermissionSessionKey)] = DateTime.Now.AddSeconds(_custStatusExpireTime).ToString();
            }
        }

        public IUserSession UserSession
        {
            get
            {
                if (_userSession == null)
                {
                    _userSession = UserSessionService.GetUserSessionInstance(_httpSession.SessionID);
                }

                return _userSession;
            }
        }
    }
}