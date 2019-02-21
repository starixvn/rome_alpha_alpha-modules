namespace Fanex.BetDetails.BetBuilder.Services
{
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.ViewModels;

    public interface IBetBuilderService
    {
        BetBuilderDetailViewModel GetBetListDetail(IGetBetBuilderDetailParameter criteria);
    }
}
