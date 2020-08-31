namespace Alpha.NetThrottling.Filters
{
    using Alpha.NetThrottling.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class WhitelistSessionFilter : IFilter
    {
        private static IEnumerable<int> configurationCustIds = ParseConfigurationCustIds();
        private readonly IEnumerable<int> custIds;

        static IEnumerable<int> ParseConfigurationCustIds()
        {
            return string.IsNullOrWhiteSpace(WhitelistConfiguration.Default.CustIds) 
                ? Enumerable.Empty<int>() 
                : WhitelistConfiguration.Default.CustIds.Split(',').Select(int.Parse).ToList();
        }

        public WhitelistSessionFilter()
            : this(null)
        {
        }

        public WhitelistSessionFilter(IEnumerable<int> custIds)
        {
            this.custIds = custIds ?? configurationCustIds;
        }

        public string GetIdentity(HttpContextBase httpContext)
        {
            var userSession = UserSessionFactory.Create();
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
