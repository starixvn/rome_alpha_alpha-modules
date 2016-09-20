namespace Alpha.Auth.DataAccesses.Impl
{
    using System.Collections;
    using Alpha.Auth.Repositories;
    using Fanex.Data;

    public class StatisticDataAccess : IStatisticDataAccess
    {
        public void UpdateFunctionUsage(
            string pageUrl,
            int serverId,
            string function,
            string subFunction,
            string site,
            string lang)
        {
            dynamic parameters = new Hashtable
            {
                { "pageurl", pageUrl },
                { "serverid", serverId },
                { "function", function },
                { "subfunction", subFunction },
                { "site", site },
                { "lang", lang }
            };

            using (var objectDb = new ObjectDb("IS_UpdFunctionUsage"))
            {
                objectDb.ExecuteNonQuery(parameters);
            }
        }
    }
}