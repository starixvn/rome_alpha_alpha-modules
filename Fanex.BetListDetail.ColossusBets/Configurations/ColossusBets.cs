namespace Fanex.BetListDetail.ColossusBets.Configurations
{
    using System.Collections.Generic;
    using System.Linq;

    public class ColossusBets
    {        
        public string SiteIds { get; set; }
        
        public string TokenAddress { get; set; }
        
        public string MemberChoiceAddress { get; set; }
        
        public IEnumerable<DomainInformation> DomainsMapping { get; set; }

        public string GetDomainBySiteId(string siteId)
        {
            var domainInfo = DomainsMapping.SingleOrDefault(i => i.SiteId.Equals(siteId));

            return domainInfo != null ? domainInfo.Domain : string.Empty;
        }
    }
}
