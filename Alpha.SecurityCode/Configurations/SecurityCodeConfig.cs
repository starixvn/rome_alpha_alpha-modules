namespace Alpha.SecurityCode.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SecurityCodeConfig
    {
        [DataMember(Name = "isActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "paths")]
        public IEnumerable<string> Paths { get; set; }

        [DataMember(Name = "sites", IsRequired = false)]
        public IEnumerable<string> Sites { get; set; }

        [DataMember(Name = "siteMainApp", IsRequired = false)]
        public string SiteMainApp { get; set; }

        [DataMember(Name = "verifyingSecurityCodePath")]
        public string VerifyingSecurityCodePath { get; set; }

        [DataMember(Name = "resetSecurityCodePath")]
        public string ResetSecurityCodePath { get; set; }

        [DataMember(Name = "verifyingOtpPath")]
        public string VerifyingOtpPath { get; set; }

        [DataMember(Name = "isSiteMain")]
        public bool IsSiteMain { get; set; }

    }
}