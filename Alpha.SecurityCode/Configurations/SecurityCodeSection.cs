namespace Alpha.SecurityCode.Configurations
{
    using System.Configuration;

    public class SecurityCodeSection : ConfigurationSection
    {
        private const string ActiveAttributeName = "active";
        private const string SecurityCodeVerifyUrlAttributeName = "securityCodeVerifyUrl";
        private const string ForceChangeSecurityCodeUrlAttributeName = "forceChangeSecurityCodeUrl";
        private const string ForceChangeSecurityCodeWithoutValidationUrlAttributeName = "forceChangeSecurityCodeWithoutValidationUrl";
        private const string IgnoreUrlsElementName = "ignoreUrl";
        private const string SiteMainName = "siteMain";
        private const string CurrentSiteName = "currentSite";

        private static SecurityCodeSection instance
            = ConfigurationManager.GetSection("SecurityCodeSection") as SecurityCodeSection;

        public static SecurityCodeSection Instance
        {
            get
            {
                return instance;
            }
        }

        [ConfigurationProperty(ActiveAttributeName, DefaultValue = "true", IsRequired = false)]
        public bool Active
        {
            get
            {
                return (bool)this[ActiveAttributeName];
            }
        }

        [ConfigurationProperty(SiteMainName, IsRequired = true)]
        public string SiteMain
        {
            get
            {
                return (string)this[SiteMainName];
            }
        }

        [ConfigurationProperty(CurrentSiteName, IsRequired = true)]
        public string CurrentSite
        {
            get
            {
                return (string)this[CurrentSiteName];
            }
        }

        [ConfigurationProperty(SecurityCodeVerifyUrlAttributeName, DefaultValue = "SecurityCode/VerifySecurityCode", IsRequired = false)]
        public string SecurityCodeVerifyUrl
        {
            get
            {
                return (string)this[SecurityCodeVerifyUrlAttributeName];
            }
        }

        [ConfigurationProperty(ForceChangeSecurityCodeUrlAttributeName, DefaultValue = "SecurityCode/ForceChangeSecurityCode", IsRequired = false)]
        public string ForceChangeSecurityCodeUrl
        {
            get
            {
                return (string)this[ForceChangeSecurityCodeUrlAttributeName];
            }
        }

        [ConfigurationProperty(ForceChangeSecurityCodeWithoutValidationUrlAttributeName, DefaultValue = "SecurityCode/ForceChangeSecurityCodeWithoutValidation", IsRequired = false)]
        public string ForceChangeSecurityCodeWithoutValidationUrl
        {
            get
            {
                return (string)this[ForceChangeSecurityCodeWithoutValidationUrlAttributeName];
            }
        }

        [ConfigurationProperty(IgnoreUrlsElementName, IsDefaultCollection = false, IsRequired = false)]
        [ConfigurationCollection(typeof(IgnoreUrlCollection))]
        public IgnoreUrlCollection IgnoreUrls
        {
            get
            {
                return (IgnoreUrlCollection)base[IgnoreUrlsElementName];
            }
        }
    }
}