namespace Alpha.NetThrottling.Filters
{
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class WhitelistSessionFitler : IFilter
    {
        private static IEnumerable<int> configurationCustIds = ParseConfigurationCustIds();

        private readonly IUserSession userSession;
        private readonly IEnumerable<int> custIds;

        static IEnumerable<int> ParseConfigurationCustIds()
        {
            var custIds = WhitelistConfiguration.Current.CustIds;
            return string.IsNullOrWhiteSpace(custIds) ? Enumerable.Empty<int>() : custIds.Split(',').Select(int.Parse).ToList();
        }

        public WhitelistSessionFitler()
            : this(null, null)
        {
        }

        public WhitelistSessionFitler(IUserSession userSession, IEnumerable<int> custIds)
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

            return $"{httpContext.Request.Path}_{custId}";
        }
    }
}
