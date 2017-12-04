using System.Web;
using System.Web.SessionState;

namespace Fanex.Products.Utilities
{
    internal static class SessionUtility
    {
        private static HttpSessionState HttpSession
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        public static T GetSession<T>(string key)
        {
            return GetSession<T>(key, default(T));
        }

        public static T GetSession<T>(string key, T defaultValue)
        {
            var session = HttpSession;
            object value = session[session.SessionID + key];

            if (value == null)
            {
                return defaultValue;
            }

            return (T)value;
        }

        public static void SetSession<T>(string key, T value)
        {
            var session = HttpSession;

            session[session.SessionID + key] = value;
        }

        public static void RemoveAll()
        {
            var session = HttpSession;

            // Session.RemoveAll(): Calling Clear() internally.
            session.RemoveAll();

            // Raise Session_End() event.
            session.Abandon();
        }
    }
}
