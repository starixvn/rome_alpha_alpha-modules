using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.Models;
using System.Collections.Generic;

namespace Fanex.BetListDetail.Lucky3.DataAccesses
{
    public interface ILucky3DataAccess
    {
        IEnumerable<Lucky3DetailModel> GetLucky3Detail(GetLucky3DetailParameter criteria);
    }
}
