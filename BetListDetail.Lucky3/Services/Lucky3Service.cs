using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.DataAccesses;
using Fanex.BetListDetail.Lucky3.Models;
using System.Collections.Generic;

namespace Fanex.BetListDetail.Lucky3.Services
{
    public class Lucky3Service : ILucky3Service
    {
        private readonly ILucky3DataAccess lucky3DataAccess;

        public Lucky3Service(ILucky3DataAccess lucky3DataAccess)
        {
            this.lucky3DataAccess = lucky3DataAccess;
        }

        public IEnumerable<Lucky3DetailModel> GetBetListDetail(GetLucky3DetailCriteria criteria)
            => lucky3DataAccess.GetLucky3Detail(criteria);
    }
}
