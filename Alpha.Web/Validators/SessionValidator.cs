namespace Alpha.Web.Validators
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using Configurations;
    using Sunplus.Agent.Model.Entities;

    public static class SessionValidator
    {
        private static string SessionVerificationTokenName
        {
            get
            {
                return AppSettings.SessionVerificationTokenCookie + HttpContext.Current.Session.SessionID;
            }
        }

        /// <summary>
        /// Just log for information, after monitor log,
        /// We will decide to kickout user or not.
        /// for case user session mixed up.
        /// </summary>
        public static void Validate()
        {
            if (!AppSettings.EnableSessionValidation)
            {
                return;
            }

            var request = HttpContext.Current.Request;
            var cookieValue = request.Cookies[SessionVerificationTokenName];

            if (cookieValue != null
                && !string.IsNullOrWhiteSpace(cookieValue.Value)
                && cookieValue.Value != GetEncodedSessionInformation())
            {
                try
                {
                    var decryptedValue = Decode(cookieValue.Value);

                    ValidationLogger.WriteWarning("Session validation failed. Cached information:" + decryptedValue);
                }
                catch (Exception ex)
                {
                    ValidationLogger.WriteWarning(ex, "Session validation failed. Cached information" + cookieValue.Value);
                }

                if (AppSettings.EnableKickOutWhenSessionInvalid)
                {
                    var session = HttpContext.Current.Session;

                    session.Clear();
                    session.Abandon();
                    if (!session.IsCookieless)
                    {
                        FormsAuthentication.SignOut();
                    }

                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                }
            }
        }

        public static void AddSessionVerificationToken()
        {
            if (!AppSettings.EnableSessionValidation)
            {
                return;
            }

            var encodedValue = GetEncodedSessionInformation();
            var httpCookie = new HttpCookie(SessionVerificationTokenName, encodedValue)
            {
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        private static string GetEncodedSessionInformation()
        {
            var rawData = string.Join(
               "_",
               UserSession.Current.UserName,
               UserSession.Current.UserId.ToString(),
               UserSession.Current.SubAccId.ToString());

            return Encode(rawData.ToUpper());
        }

        private static string Encode(string sessionInfo)
        {
            var encodedSessionInfo = Encoding.UTF8.GetBytes(sessionInfo);

            return HttpServerUtility.UrlTokenEncode(encodedSessionInfo);
        }

        private static string Decode(string encodedSessionInfo)
        {
            var decodedSessionInfo = HttpServerUtility.UrlTokenDecode(encodedSessionInfo);

            return Encoding.UTF8.GetString(decodedSessionInfo);
        }
    }
}