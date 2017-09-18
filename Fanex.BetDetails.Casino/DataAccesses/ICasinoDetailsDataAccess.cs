namespace Fanex.BetDetails.Casino.DataAccesses
{
    using System.Threading.Tasks;
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailsDataAccess
    {
        Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailsParam param);
    }
}
