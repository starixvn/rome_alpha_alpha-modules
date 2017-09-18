namespace Fanex.BetListDetail.Casino.Services
{
    using System.Threading.Tasks;
    using BetList.Core.Entities;
    using DataAccesses;
    using Models;
    using Params;

    public class CasinoDetailService : ICasinoDetailService
    {
        private readonly ICasinoDetailDataAccess casinoDetailDataAccess;

        public CasinoDetailService(ICasinoDetailDataAccess casinoDetailDataAccess)
        {
            this.casinoDetailDataAccess = casinoDetailDataAccess;
        }

        public async Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailParam param)
            => await casinoDetailDataAccess.GetCasinoDetail(param);
    }
}
