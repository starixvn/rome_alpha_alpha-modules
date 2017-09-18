namespace Fanex.BetListDetail.Casino.Services
{
    using System.Threading.Tasks;
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailService
    {
        Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailParam param);
    }
}
