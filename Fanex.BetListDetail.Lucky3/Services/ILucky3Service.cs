namespace Fanex.BetListDetail.Lucky3.Services
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using ViewModels;

    public interface ILucky3Service
    {
        Lucky3DetailViewModel GetBetListDetail(IGetLucky3DetailParameter criteria);
    }
}
