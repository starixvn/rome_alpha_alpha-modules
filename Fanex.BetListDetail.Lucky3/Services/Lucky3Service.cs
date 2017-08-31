namespace Fanex.BetListDetail.Lucky3.Services
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.DataAccesses;
    using Fanex.BetListDetail.Lucky3.Models;
    using System.Collections.Generic;

    public class Lucky3Service : ILucky3Service
    {
        private readonly ILucky3DataAccess lucky3DataAccess;

        public Lucky3Service(ILucky3DataAccess lucky3DataAccess)
        {
            this.lucky3DataAccess = lucky3DataAccess;
        }

        public IEnumerable<Lucky3DetailModel> GetBetListDetail(IGetLucky3DetailParameter criteria)
            => lucky3DataAccess.GetLucky3Detail(criteria);
    }
}
