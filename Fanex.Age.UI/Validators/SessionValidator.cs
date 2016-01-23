namespace Fanex.Age.UI.Validators
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using Fanex.Common;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public static class SessionValidator
    {
        private const string SessionVerificationTokenNameFormat = "__SessionVerificationToken_{0}";
        private static readonly bool _enableSessionValidation = true;

        static SessionValidator()
        {
            var enableSessionValidation = ConfigurationManager.AppSettings["EnableSessionValidation"];
            if (!string.IsNullOrWhiteSpace(enableSessionValidation))
            {
                _enableSessionValidation = enableSessionValidation != "0";
            }
        }

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
            if (!_enableSessionValidation)
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

                    ValidationLogger.WriteWarning(string.Join(
                        "\r\n",
                        "Session validation failed. Cached information:" + decryptedValue));
                }
                catch (Exception ex)
                {
                    ValidationLogger.WriteWarning(ex, "Session validation failed. Cached information" + cookieValue.Value);
                }
            }
        }

        public static void AddVerifyUsernameToken()
        {
            if (!_enableSessionValidation)
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