namespace Fanex.Age.UI.Validators
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public static class ValidationLogger
    {
        public static void WriteWarning(string message)
        {
            string logMessage = BuildMessage(message);

            var validationException = new Exception(logMessage);

            Logs.WriteWarn(validationException);
        }

        public static void WriteWarning(Exception ex, string message)
        {
            string logMessage = BuildMessage(message);

            var validationException = new Exception(logMessage, ex);

            Logs.WriteWarn(validationException);
        }

        private static string BuildMessage(string message)
        {
            NameValueCollection headers = HttpContext.Current.Request.Headers;
            var headerInfo = "Header Information: \r\n";
            for (int i = 0; i < headers.Count; i++)
            {
                headerInfo += headers.GetKey(i) + " = " + headers.Get(i) + "\r\n";
            }

            return string.Join("\r\n",
                                    message,
                                    "Cust Id: " + UserSession.Current.CustId.ToString(),
                                    "Username: " + UserSession.Current.UserName,
                                    "Sub Account Id: " + UserSession.Current.SubAccId,
                                    "Session Id: " + HttpContext.Current.Session.SessionID,
                                    headerInfo,
                                    "Request Body: " + GetRequestBody(HttpContext.Current.Request));
        }

        private static string GetRequestBody(HttpRequest request)
        {
            Stream req = request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(req);

            return streamReader.ReadToEnd();
        }
    }
}