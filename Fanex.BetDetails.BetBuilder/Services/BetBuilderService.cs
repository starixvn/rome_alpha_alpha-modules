namespace Fanex.BetDetails.BetBuilder.Services
{
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.DataAccesses;
    using Fanex.BetDetails.BetBuilder.ViewModels;

    public class BetBuilderService : IBetBuilderService
    {
        private readonly IBetBuilderDataAccess betBuilderDataAccess;

        public BetBuilderService(IBetBuilderDataAccess betBuilderDataAccess)
        {
            this.betBuilderDataAccess = betBuilderDataAccess;
        }

        public BetBuilderDetailViewModel GetBetListDetail(IGetBetBuilderDetailParameter criteria)        
            => new BetBuilderDetailViewModel(betBuilderDataAccess.GetBetBuilderDetail(criteria), criteria.Status);
    }
}
