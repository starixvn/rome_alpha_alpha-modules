namespace Alpha.SecurityCode.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Web;

    [DataContract]
    public class SecurityCodeConfig
    {
        [DataMember(Name = "isActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "urls")]
        public IEnumerable<string> Urls { get; set; }

        [DataMember(Name = "sites")]
        public IEnumerable<string> Sites { get; set; }

        [DataMember(Name = "siteMainApp")]
        public string SiteMainApp { get; set; }

        [DataMember(Name = "currentSiteApp")]
        public string CurrentSiteApp { get; set; }

        [DataMember(Name = "verifyingSecurityCodeUrl")]
        public string VerifyingSecurityCodeUrl { get; set; }

        [DataMember(Name = "changeSecurityCodeUrl")]
        public string ChangeSecurityCodeUrl { get; set; }

        [DataMember(Name = "resetSecurityCodeUrl")]
        public string ResetSecurityCodeUrl { get; set; }

    }
}