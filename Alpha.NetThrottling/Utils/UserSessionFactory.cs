namespace Alpha.NetThrottling.Utils
{
    using Sunplus.Agent.Model.Entities;

    public static class UserSessionFactory
    {
        public static IUserSession Create()
        {
            return UserSession.Current;
        }
    }
}
