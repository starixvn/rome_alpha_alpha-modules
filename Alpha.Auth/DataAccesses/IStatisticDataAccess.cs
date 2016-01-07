namespace Alpha.Auth.Repositories
{
    public interface IStatisticDataAccess
    {
        void UpdatePageVisit(int roleId, string pageUrl);

        void UpdateFunctionUsage(
            string pageUrl,
            int serverId,
            string function,
            string subFunction,
            string site,
            string lang);

        void UpdateFunctionUsage(int serverId, string function, string subFunction, string lang);
    }
}