namespace Fanex.BetListDetail.Lucky3.Services
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.DataAccesses;
    using ViewModels;

    public class Lucky3Service : ILucky3Service
    {
        private readonly ILucky3DataAccess lucky3DataAccess;

        public Lucky3Service(ILucky3DataAccess lucky3DataAccess)
        {
            this.lucky3DataAccess = lucky3DataAccess;
        }

        public Lucky3DetailViewModel GetBetListDetail(IGetLucky3DetailParameter criteria)
            => new Lucky3DetailViewModel(lucky3DataAccess.GetLucky3Detail(criteria), criteria.LevelId);
    }
}
