namespace Alpha.Auth.Services.Impl
{
    using System.Web;
    using DataAccesses.Impl;
    using Repositories;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class StatisticService : IStatisticService
    {
        private readonly IUserSession _userSession;
        private readonly IStatisticDataAccess _dataAccess;

        public StatisticService()
        {
            _userSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            _dataAccess = new StatisticDataAccess();
        }

        public void UpdatePageVisit(string url)
        {
            _dataAccess.UpdatePageVisit(_userSession.RoleId, url);
        }

        public void UpdateFunctionUsage(string url, string function, string subFunction, string site)
        {
            _dataAccess.UpdateFunctionUsage(url, _userSession.ServerId, function, subFunction, site, _userSession.LangKey);
        }

        public void UpdateFunctionUsage(string function, string subFunction)
        {
            _dataAccess.UpdateFunctionUsage(_userSession.ServerId, function, subFunction, _userSession.LangKey);
        }
    }
}