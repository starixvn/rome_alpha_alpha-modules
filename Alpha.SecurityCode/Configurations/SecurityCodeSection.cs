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

        private static SecurityCodeSection instance
            = ConfigurationManager.GetSection("AlphaSecurityCodeSection") as SecurityCodeSection;

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

        [ConfigurationProperty(SecurityCodeVerifyUrlAttributeName, DefaultValue = "/site-main/Security/SecurityCode/VerifySecurityCode", IsRequired = false)]
        public string SecurityCodeVerifyUrl
        {
            get
            {
                return (string)this[SecurityCodeVerifyUrlAttributeName];
            }
        }

        [ConfigurationProperty(ForceChangeSecurityCodeUrlAttributeName, DefaultValue = "/site-main/Security/SecurityCode/ForceChangeSecurityCode", IsRequired = false)]
        public string ForceChangeSecurityCodeUrl
        {
            get
            {
                return (string)this[ForceChangeSecurityCodeUrlAttributeName];
            }
        }

        [ConfigurationProperty(ForceChangeSecurityCodeWithoutValidationUrlAttributeName, DefaultValue = "/site-main/Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation", IsRequired = false)]
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