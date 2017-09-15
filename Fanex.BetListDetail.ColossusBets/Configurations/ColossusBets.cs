namespace Fanex.BetListDetail.ColossusBets.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Linq;

    [DataContract]
    public class ColossusBets
    {
        [DataMember(Name = "SiteIds")]
        public string SiteIds { get; set; }

        [DataMember(Name = "ColossusBetsAPI_GetToken")]
        public string GetTokenAddress { get; set; }

        [DataMember(Name = "ColossusBetsAPI_MemberChoice")]
        public string GetMemberChoiceAddress { get; set; }

        [DataMember(Name = "DomainsMapping")]
        public IEnumerable<DomainInformation> Domains { get; set; }

        public string GetDomainBySiteId(string siteId)
        {
            var domainInfo = Domains.SingleOrDefault(i => i.SiteId.Equals(siteId));

            return domainInfo != null ? domainInfo.Domain : string.Empty;
        }
    }
}
