namespace Alpha.Auth.Web.Validators
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Configurations;
    using Fanex.Common;
    using Sunplus.Agent.Model.Entities;

    public static class SessionValidator
    {
        private const string SessionVerificationTokenNameFormat = "__SessionVerificationToken_{0}";

        private static string SessionVerificationTokenName
        {
            get
            {
                return string.Format(SessionVerificationTokenNameFormat, HttpContext.Current.Session.SessionID);
            }
        }

        /// <summary>
        /// Just log for information, after monitor log,
        /// We will decide to kickout user or not.
        /// for case user session mixed up.
        /// </summary>
        private static void VerifyUsername()
        {
            if (!AlphaAuthSection.Instance.EnableSessionValidation)
            {
                return;
            }

            var request = HttpContext.Current.Request;
            var cookieValue = request.Cookies[SessionVerificationTokenName];

            if (cookieValue != null
                && !string.IsNullOrWhiteSpace(cookieValue.Value)
                && cookieValue.Value != GetEncryptedSessionInformation())
            {
                try
                {
                    var decryptedValue = Cryptography.CfsDeCrypt(cookieValue.Value);

                    ValidationLogger.WriteWarning("Session validation failed. Cached information:" + decryptedValue);
                }
                catch (Exception ex)
                {
                    ValidationLogger.WriteWarning(ex, "Session validation failed. Cached information" + cookieValue.Value);
                }

                if (AlphaAuthSection.Instance.EnableKickOutWhenSessionInvalid)
                {
                    var session = HttpContext.Current.Session;

                    session.Clear();
                    session.Abandon();
                    FormsAuthentication.SignOut();

                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                }
            }
        }

        public static void AddSessionVerificationToken()
        {
            if (!AlphaAuthSection.Instance.EnableSessionValidation)
            {
                return;
            }

            var encryptedValue = GetEncryptedSessionInformation();
            var httpCookie = new HttpCookie(SessionVerificationTokenName, encryptedValue)
            {
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        private static string GetEncryptedSessionInformation()
        {
            var rawData = string.Join(
               "_",
               UserSession.Current.UserName,
               UserSession.Current.UserId.ToString(),
               UserSession.Current.SubAccId.ToString());

            return Cryptography.CfsEnCrypt(rawData);
        }
    }
}