namespace Alpha.Auth.Services.Impl
{
    using DataAccesses.Impl;
    using Models;
    using Repositories;

    public class StatisticService : IStatisticService
    {
        private readonly ICustContext _custContext;
        private readonly IStatisticDataAccess _dataAccess;

        public StatisticService(
            ICustContext custContext = null,
            IStatisticDataAccess dataAccess = null)
        {
            _custContext = custContext ?? new CustContext();
            _dataAccess = dataAccess ?? new StatisticDataAccess();
        }

        public void UpdateFunctionUsage(string url, string function, string subFunction, string site)
        {
            _dataAccess.UpdateFunctionUsage(url, _custContext.UserSession.ServerId, function, subFunction, site, _custContext.UserSession.LangKey);
        }
    }
}