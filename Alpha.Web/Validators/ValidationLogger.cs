namespace Alpha.Web.Validators
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.Entities;

    public static class ValidationLogger
    {
        public static void WriteWarning(string message)
        {
            string logMessage = BuildMessage(message);

            var validationException = new Exception(logMessage);

            ExceptionHandler.Handle(validationException, TraceEventType.Warning);
        }

        public static void WriteWarning(Exception ex, string message)
        {
            string logMessage = BuildMessage(message);

            var validationException = new Exception(logMessage, ex);

            ExceptionHandler.Handle(validationException, TraceEventType.Warning);
        }

        public static void WriteInfo(string message)
        {
            string logMessage = BuildMessage(message);

            var validationException = new Exception(logMessage);

            ExceptionHandler.Handle(validationException, TraceEventType.Information);
        }

        private static string BuildMessage(string message)
        {
            NameValueCollection headers = HttpContext.Current.Request.Headers;
            var headerInfo = "Header Information: \r\n";
            for (int i = 0; i < headers.Count; i++)
            {
                headerInfo += headers.GetKey(i) + " = " + headers.Get(i) + "\r\n";
            }

            return string.Join(
                "\r\n",
                message,
                "Cust Id: " + UserSession.Current.CustId.ToString(),
                "Username: " + UserSession.Current.UserName,
                "Sub Account Id: " + UserSession.Current.SubAccId,
                "Session Id: " + HttpContext.Current.Session.SessionID,
                headerInfo);
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