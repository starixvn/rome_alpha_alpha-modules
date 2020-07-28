namespace Alpha.NetThrottling.Filters
{
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class WhitelistSessionFilter : IFilter
    {
        private static IEnumerable<int> configurationCustIds = ParseConfigurationCustIds();
        private readonly IEnumerable<int> custIds;

        static IEnumerable<int> ParseConfigurationCustIds()
        {
            var custIds = WhitelistConfiguration.Current.CustIds;
            return string.IsNullOrWhiteSpace(custIds) ? Enumerable.Empty<int>() : custIds.Split(',').Select(int.Parse).ToList();
        }

        public WhitelistSessionFilter()
            : this(null)
        {
        }

        public WhitelistSessionFilter(IEnumerable<int> custIds)
        {
            this.custIds = custIds ?? configurationCustIds;
        }

        public string GetIdentity(HttpContext httpContext)
        {
            var userSession = UserSession.Current;
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
