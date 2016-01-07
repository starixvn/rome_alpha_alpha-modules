namespace Alpha.Auth.Repositories
{
    public interface IStatisticDataAccess
    {
        void Age_UpdPageVisit(int roleId, string pageUrl);

        void IS_UpdFunctionUsage(
            string pageUrl,
            int serverId,
            string function,
            string subFunction,
            string site,
            string lang);

        void Agm_UpdFunctionUsage(int serverId, string function, string subFunction, string lang);
    }
}