namespace Fanex.BetDetails.Casino.Services
{
    using BetList.Core.Entities;
    using DataAccesses;
    using Models;
    using Params;

    public class CasinoDetailsService : ICasinoDetailsService
    {
        private readonly ICasinoDetailsDataAccess casinoDetailDataAccess;

        public CasinoDetailsService(ICasinoDetailsDataAccess casinoDetailDataAccess)
        {
            this.casinoDetailDataAccess = casinoDetailDataAccess;
        }

        public PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param)
            => casinoDetailDataAccess.GetCasinoDetails(param);
    }
}
