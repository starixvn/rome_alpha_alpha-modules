namespace Fanex.BetDetails.Casino.Services
{
    using System.Threading.Tasks;
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

        public async Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailsParam param)
            => await casinoDetailDataAccess.GetCasinoDetail(param);
    }
}
