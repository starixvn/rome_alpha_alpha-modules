namespace Sunplus.Agent.Utilities
{
    using System;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Fanex.Logging;

    public static class Logs
    {
        public static void Write(Exception ex, int customerId = 0)
        {
            string error = FullExceptionToString(ex, customerId);
            if (ex is ThreadAbortException)
            {
                Warn(error);
            }
            else
            {
                Error(error);
            }
        }

        public static void FilterInfoWarnOrError(Exception ex, int customerId = 0)
        {
            if (LogsFilter.IsInfoLog(ex.Message))
            {
                WriteInfo(ex, customerId);
            }
            else if (LogsFilter.IsWarnLog(ex.Source))
            {
                WriteWarn(ex, customerId);
            }
            else
            {
                Write(ex, customerId);
            }
        }

        public static void WriteInfo(string message)
        {
            Info(message);
        }

        public static void WriteInfo(Exception ex, int customerId = 0)
        {
            string info = FullExceptionToString(ex, customerId);
            Info(info);
        }

        public static void WriteWarn(Exception ex, int customerId = 0)
        {
            string warn = FullExceptionToString(ex, customerId);
            Warn(warn);
        }

        #region LogService members and Helper methods

        private static void Info(string message)
        {
            LogService.Info(message);
        }

        private static void Warn(string message)
        {
            LogService.Warn(message);
        }

        private static void Error(string message)
        {
            LogService.Error(message);
        }

        public static string FullExceptionToString(Exception ex, int customerId = 0)
        {
            if (ex == null || HttpContext.Current == null)
            {
                return string.Empty;
            }

            HttpRequest httpRequest = HttpContext.Current.Request;
            return string.Join(
                "\r\n",
                ExceptionToString(ex, httpRequest, customerId),
                "EXCEPTION DATA:" + ExceptionDataToString(ex.Data),
                "BROWSER CAPABILITIES:" + BrowserInformation(httpRequest.Browser),
                SqlExceptionToString(ex));
        }

        private static string ExceptionToString(Exception ex, HttpRequest httpRequest, int customerId)
        {
            string url = LogsFilter.CensorSensitiveInformation(httpRequest.Url.OriginalString);

            string formString = string.Empty;
            if (httpRequest.Form.Count > 0)
            {
                formString = httpRequest.Form.ToString();
                formString = LogsFilter.CensorSensitiveInformation(formString);
            }

            string message = string.Join(
                "\r\n",
                ex.Message,
                "URL: " + url,
                "REFERRER: " + httpRequest.UrlReferrer,
                "FORM: " + formString,
                "SOURCE: " + ex.Source,
                "STACKTRACE: " + ex.StackTrace,
                "TARGETSITE: " + ex.TargetSite,
                "DETAIL: " + ex,
                "CULTURE: " + Thread.CurrentThread.CurrentUICulture,
                "CLIENT IP: " + CustomerIp());

            if (customerId != 0)
            {
                message += "\r\nCUSTID: " + customerId;
            }

            return message;
        }

        private static string SqlExceptionToString(Exception ex)
        {
            if (ex is SqlException)
            {
                return GetSqlExceptionInfo((SqlException)ex);
            }

            if (ex.InnerException is SqlException)
            {
                return GetSqlExceptionInfo((SqlException)ex.InnerException);
            }

            return string.Empty;
        }

        private static string GetSqlExceptionInfo(SqlException ex)
        {
            return string.Join(
                "\r\n",
                    "PROCEDURE: " + ex.Procedure,
                    "LINE NUMBER: " + ex.LineNumber,
                    "SERVER: " + ex.Server);
        }

        private static string ExceptionDataToString(IDictionary dictionary)
        {
            string message = string.Empty;
            if (dictionary != null)
            {
                message = dictionary.Keys.Cast<object>().Aggregate(message, (current, key) => current + string.Format("\r\n{0}: {1}", key, dictionary[key]));
            }

            return message;
        }

        private static string BrowserInformation(HttpBrowserCapabilities browser)
        {
            return browser == null
                ? string.Empty
                : string.Join(
                    "\r\n",
                    "Type: " + browser.Type,
                    "Name: " + browser.Browser,
                    "Version: " + browser.Version,
                    "Major Version: " + browser.MajorVersion,
                    "Minor Version: " + browser.MinorVersion,
                    "Platform: " + browser.Platform,
                    "Is Beta: " + browser.Beta,
                    "Is Crawler: " + browser.Crawler,
                    "Is AOL: " + browser.AOL,
                    "Is Win16: " + browser.Win16,
                    "Is Win32: " + browser.Win32,
                    "Supports Frames: " + browser.Frames,
                    "Supports Tables: " + browser.Tables,
                    "Supports Cookies: " + browser.Cookies);
        }

        private static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip.Equals("::1") ? "127.0.0.1" : (string.IsNullOrEmpty(ip) ? "127.0.0.1" : ip.Split(',')[0]);
        }
        #endregion LogService members and Helper methods
    }
}