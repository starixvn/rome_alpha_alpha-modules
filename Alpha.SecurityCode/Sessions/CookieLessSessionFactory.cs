namespace Alpha.SecurityCode.Sessions
{
    using System.Web;

    public static class CookieLessSessionFactory
    {
        public static ISecurityCodeSession SecurityCodeSession 
        {
            get
            {
                return new SecurityCodeSession(HttpContext.Current.Session.SessionID);
            }
        }
    }
}
