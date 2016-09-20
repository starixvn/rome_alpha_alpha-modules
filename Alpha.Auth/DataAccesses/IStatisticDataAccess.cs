namespace Alpha.Auth.Repositories
{
    public interface IStatisticDataAccess
    {
        void UpdateFunctionUsage(
            string pageUrl,
            int serverId,
            string function,
            string subFunction,
            string site,
            string lang);
    }
}