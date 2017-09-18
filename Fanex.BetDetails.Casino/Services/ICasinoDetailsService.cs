namespace Fanex.BetDetails.Casino.Services
{
    using System.Threading.Tasks;
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailsService
    {
        Task<PagedData<ITicket>> GetCasinoDetail(CasinoDetailsParam param);
    }
}
