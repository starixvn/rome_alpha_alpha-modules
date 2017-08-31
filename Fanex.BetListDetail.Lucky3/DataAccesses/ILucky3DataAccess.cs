namespace Fanex.BetListDetail.Lucky3.DataAccesses
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.Models;
    using System.Collections.Generic;

    public interface ILucky3DataAccess
    {
        IEnumerable<Lucky3DetailModel> GetLucky3Detail(IGetLucky3DetailParameter criteria);
    }
}
