namespace Fanex.BetListDetail.ColossusBets.Configurations
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DomainInformation
    {
        [DataMember(Name = "SiteId")]
        public string SiteId { get; set; }

        [DataMember(Name = "DomainName")]
        public string Domain { get; set; }
    }
}
