namespace Alpha.Auth.DataAccesses.Impl
{
    using System.Collections;
    using Alpha.Auth.Repositories;
    using Sunplus.Agent.Model.DL.Base;

    public class StatisticDataAccess : DAOBase, IStatisticDataAccess
    {
        public void Age_UpdPageVisit(int roleId, string pageUrl)
        {
            var parameters = new Hashtable
            {
                { "role", roleId },
                { "pageurl", pageUrl }
            };
            ExecSp("Age_UpdPageVisit", parameters);
        }

        public void IS_UpdFunctionUsage(
            string pageUrl,
            int serverId,
            string function,
            string subFunction,
            string site,
            string lang)
        {
            var parameters = new Hashtable
            {
                { "pageurl", pageUrl },
                { "serverid", serverId },
                { "function", function },
                { "subfunction", subFunction },
                { "site", site },
                { "lang", lang }
            };

            ExecSp("IS_UpdFunctionUsage", parameters);
        }

        public void Agm_UpdFunctionUsage(int serverId, string function, string subFunction, string lang)
        {
            var parameters = new Hashtable
            {
                { "serverid", serverId },
                { "function", function },
                { "subfunction", subFunction },
                { "lang", lang }
            };

            ExecSp("Agm_UpdFunctionUsage", parameters);
        }
    }
}