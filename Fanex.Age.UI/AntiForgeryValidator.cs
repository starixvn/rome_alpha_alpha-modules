namespace Fanex.Age.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Helpers;
    using Fanex.Common;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public static class AntiForgeryValidator
    {
        private const string AntiForgeryCookieName = "__AntiForgeryCookieName";
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";
        private const string UsernameTokenName = "__UsernameVerificationToken";

        public static bool ValidateRequest()
        {
            try
            {
                var request = HttpContext.Current.Request;
                var cookieName = request.Headers[AntiForgeryCookieName];
                var cookieValue = request.Cookies[cookieName];
                var formToken = request.Headers[AntiForgeryFormTokenName];

                if (cookieValue != null
                    && !string.IsNullOrWhiteSpace(cookieValue.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);

                    VerifyUsername();
                }
                else
                {
                    LogCookieInvalidInfor("Validation of Anti-XSRF process is failed");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteWarn(ex, UserSession.Current.CustId);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Just log for information, after monitor log, 
        /// We will decide to kickout user or not.
        /// for case user session mixed up.
        /// </summary>
        private static void VerifyUsername()
        {
            var request = HttpContext.Current.Request;
            var cookieValue = request.Cookies[UsernameTokenName];

            if (cookieValue != null
                && !string.IsNullOrWhiteSpace(cookieValue.Value)
                && cookieValue.Value != Cryptography.CfsEnCrypt(UserSession.Current.UserName))
            {
                try
                {
                    LogCookieInvalidInfor(string.Join(" ", 
                        "Username",
                        Cryptography.CfsDeCrypt(cookieValue.Value),
                        "is invalid"));
                }
                catch(Exception ex)
                {
                    LogCookieInvalidInfor(string.Join(" ",
                        "Username",
                        Cryptography.CfsDeCrypt(cookieValue.Value),
                        "is invalid",
                        "\r\n",
                        ex.Message));
                }
            }
        }

        public static void AddVerifyUsernameToken()
        {
            var encryptedValue = Cryptography.CfsEnCrypt(UserSession.Current.UserName);
            var httpCookie = new HttpCookie(UsernameTokenName, encryptedValue);
            httpCookie.HttpOnly = true;

            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        private static void LogCookieInvalidInfor(string actionName)
        {
            NameValueCollection headers = HttpContext.Current.Request.Headers;
            var headerInfo = "Header Information: \r\n";
            for (int i = 0; i < headers.Count; i++)
            {
                headerInfo += headers.GetKey(i) + " = " + headers.Get(i) + "\r\n";
            }

            var cookieDoesNotAvailableMessage = new Exception(string.Join("\r\n",
                                    actionName,
                                    string.Format("Current Cust Id: {0}", UserSession.Current.CustId),
                                    string.Format("Current Username: {0}", UserSession.Current.UserName),
                                    string.Format("Current Sub Account Id: {0}", UserSession.Current.SubAccId),
                                    string.Format("Current Session Id: {0}", HttpContext.Current.Session.SessionID),
                                    headerInfo));

            Logs.WriteWarn(cookieDoesNotAvailableMessage);
        }

        public static void RegisterAntiForgeryToken(TplPage tplPage)
        {
            string cookieToken;
            string formToken;

            var cookieName = string.Join("_",
                AntiForgeryCookieName,
                HttpContext.Current.Session.SessionID,
                tplPage.GetType().Name);

            AntiForgery.GetTokens(null, out cookieToken, out formToken);

            tplPage.RegisterJavascriptObject(AntiForgeryFormTokenName, formToken);
            tplPage.RegisterJavascriptObject(AntiForgeryCookieName, cookieName);

            HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookieName)
            {
                HttpOnly = true,
                Value = cookieToken,
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}