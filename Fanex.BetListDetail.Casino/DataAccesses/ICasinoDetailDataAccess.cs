namespace Fanex.BetListDetail.Casino.DataAccesses
{
    using System.Threading.Tasks;
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailDataAccess
    {
        Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailParam param);
    }
}
