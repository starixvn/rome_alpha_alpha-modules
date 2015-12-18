namespace Sunplus.Agent.Model.BL.Statistic
{
    using System;
    using Sunplus.Agent.Model.DL.Statistic;

    [Obsolete("Should rename to Sunplus.Agent.Model.Auth")]
    public class StatisticBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public StatisticBO(string netSessionId)
            : base(netSessionId)
        {
        }

        public void UpdatePageVisit_Old(string url)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.Age_UpdPageVisit(CurrentSession.RoleId, url);
        }

        public void UpdateFunctionUsage(string url, string function, string subFunction, string site)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.IS_UpdFunctionUsage(url, CurrentSession.ServerId, function, subFunction, site, CurrentSession.LangKey);
        }

        public void UpdateFunctionUsage(string function, string subFunction)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.Agm_UpdFunctionUsage(CurrentSession.ServerId, function, subFunction, CurrentSession.LangKey);
        }
    }
}