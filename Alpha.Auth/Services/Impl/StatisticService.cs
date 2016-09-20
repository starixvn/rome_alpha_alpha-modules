namespace Alpha.Auth.Services.Impl
{
    using DataAccesses.Impl;
    using Models;
    using Repositories;

    public class StatisticService : IStatisticService
    {
        private readonly IRequestContext _requestContext;
        private readonly IStatisticDataAccess _dataAccess;

        public StatisticService(
            IRequestContext custContext = null,
            IStatisticDataAccess dataAccess = null)
        {
            _requestContext = custContext ?? new RequestContext();
            _dataAccess = dataAccess ?? new StatisticDataAccess();
        }

        public void UpdateFunctionUsage(string url, string function, string subFunction, string site)
        {
            _dataAccess.UpdateFunctionUsage(url, _requestContext.UserSession.ServerId, function, subFunction, site, _requestContext.UserSession.LangKey);
        }
    }
}