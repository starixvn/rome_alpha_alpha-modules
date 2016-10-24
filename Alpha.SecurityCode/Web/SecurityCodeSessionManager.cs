namespace Alpha.SecurityCode.Web
{
    public static class SecurityCodeSessionManager
    {
        static SecurityCodeSessionManager()
        {
            if (SecurityCodeSession == null)
            {
                SecurityCodeSession = new SecurityCodeSessionImpl();
            }
        }

        public static SecurityCodeSession SecurityCodeSession { get; set; }
    }
}