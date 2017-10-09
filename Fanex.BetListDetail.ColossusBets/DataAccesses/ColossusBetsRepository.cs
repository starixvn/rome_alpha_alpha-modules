namespace Fanex.BetListDetail.ColossusBets.DataAccesses
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetListDetail.ColossusBets.Criteria;
    using System.Collections.Generic;

    public interface ColossusBetsRepository
    {
        IEnumerable<ITicket> BetDetails(GetColossusBetsBetDetailsParameter parameter);

        string GetSiteIdByCustomerId(int customerId);
    }
}
