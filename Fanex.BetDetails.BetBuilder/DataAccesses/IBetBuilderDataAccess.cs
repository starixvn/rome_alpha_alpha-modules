namespace Fanex.BetDetails.BetBuilder.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.ViewModels;
    using Model;

    public interface IBetBuilderDataAccess
    {
        IEnumerable<BetBuilderDetailModel> GetBetBuilderDetail(IGetBetBuilderDetailParameter criteria);
    }
}
