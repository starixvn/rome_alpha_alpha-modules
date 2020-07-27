namespace Alpha.NetThrottling.Filters
{
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class WhitelistSessionFitler : IFilter
    {
        private static List<int> configurationCustIds = WhitelistConfiguration.Current.CustIds.Split(',').Select(int.Parse).ToList();
        private readonly IUserSession userSession;
        private readonly List<int> custIds;

        public WhitelistSessionFitler()
            : this(null, null)
        {
        }

        public WhitelistSessionFitler(IUserSession userSession, List<int> custIds)
        {
            this.userSession = userSession ?? UserSession.Current;
            this.custIds = custIds ?? configurationCustIds;
        }

        public string GetIdentity(HttpContext httpContext)
        {
            if (userSession == null)
            {
                return null;
            }

            var custId = userSession.SRecommendId;
            if (custIds.Contains(custId))
            {
                return null;
            }

            return $"{custId}";
        }
    }
}
