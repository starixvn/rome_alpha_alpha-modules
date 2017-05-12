using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.Models;
using System.Collections.Generic;

namespace Fanex.BetListDetail.Lucky3.Services
{
    public interface ILucky3Service
    {
        IEnumerable<Lucky3DetailModel> GetBetListDetail(GetLucky3DetailCriteria criteria);
    }
}
