namespace Fanex.BetListDetail.Lucky3.Services
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.Models;
    using System.Collections.Generic;

    public interface ILucky3Service
    {
        IEnumerable<Lucky3DetailModel> GetBetListDetail(IGetLucky3DetailParameter criteria);
    }
}
