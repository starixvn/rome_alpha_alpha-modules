namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class AlphaAuthSection : ConfigurationSection
    {
        private const string EnableAntiXSRFName = "enableAntiXSRF";
        private const string EnableSessionValidationName = "enableSessionValidation";
        private const string EnableKickOutWhenSessionInvalidName = "enableKickOutWhenSessionInvalid";
        private const string SessionVerificationTokenCookieName = "sessionVerificationTokenCookie";

        private static AlphaAuthSection instance
            = ConfigurationManager.GetSection("AlphaAuthSection") as AlphaAuthSection;

        public static AlphaAuthSection Instance
        {
            get
            {
                return instance;
            }
        }

        [ConfigurationProperty(EnableAntiXSRFName, DefaultValue = true, IsRequired = false)]
        public bool EnableAntiXSRF
        {
            get
            {
                return (bool)this[EnableAntiXSRFName];
            }
        }

        [ConfigurationProperty(EnableSessionValidationName, DefaultValue = true, IsRequired = false)]
        public bool EnableSessionValidation
        {
            get
            {
                return (bool)this[EnableSessionValidationName];
            }
        }

        [ConfigurationProperty(EnableKickOutWhenSessionInvalidName, DefaultValue = false, IsRequired = false)]
        public bool EnableKickOutWhenSessionInvalid
        {
            get
            {
                return (bool)this[EnableKickOutWhenSessionInvalidName];
            }
        }

        [ConfigurationProperty(SessionVerificationTokenCookieName, DefaultValue = "__SessionVerificationToken_", IsRequired = false)]
        public string SessionVerificationTokenCookie
        {
            get
            {
                return (string)this[SessionVerificationTokenCookieName];
            }
        }
    }
}