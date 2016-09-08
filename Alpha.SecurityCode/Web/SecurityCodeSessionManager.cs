namespace Alpha.SecurityCode.Web
{
    public static class SecurityCodeSessionManager
    {
        public static ISecurityCodeSession SecurityCodeSession { get; internal set; }

        static SecurityCodeSessionManager()
        {
            if (SecurityCodeSession == null)
            {
                SecurityCodeSession = new SecurityCodeSession();
            }
        }
    }
}