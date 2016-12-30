namespace Alpha.Web.Configurations
{
    using System.Configuration;

    public static class AppSettings
    {
        private static bool enableKickOutWhenSessionInvalid;
        private static string sessionVerificationTokenCookie;
        private static bool enableSessionValidation;
        private static bool enableAntiXSRF;
        private static string oldMainAppPath;
        private static string mainAppPath;
        private static string assetAppPath;
        private static string staticResourcesVersion;

        static AppSettings()
        {
            enableKickOutWhenSessionInvalid = ConfigurationManager.AppSettings["EnableKickOutWhenSessionInvalid"] != null
                ? bool.Parse(ConfigurationManager.AppSettings["EnableKickOutWhenSessionInvalid"])
                : false;

            sessionVerificationTokenCookie = ConfigurationManager.AppSettings["SessionVerificationTokenCookie"] != null
                ? ConfigurationManager.AppSettings["SessionVerificationTokenCookie"]
                : "__SessionVerificationToken_";

            enableSessionValidation = ConfigurationManager.AppSettings["EnableSessionValidation"] != null
                ? bool.Parse(ConfigurationManager.AppSettings["EnableSessionValidation"])
                : false;

            enableAntiXSRF = ConfigurationManager.AppSettings["EnableAntiXSRF"] != null
                ? bool.Parse(ConfigurationManager.AppSettings["EnableAntiXSRF"])
                : true;

            oldMainAppPath = ConfigurationManager.AppSettings["OldMainAppPath"] != null
                ? ConfigurationManager.AppSettings["OldMainAppPath"]
                : "ex-main";

            mainAppPath = ConfigurationManager.AppSettings["MainAppPath"] != null
                ? ConfigurationManager.AppSettings["MainAppPath"]
                : "site-main";

            assetAppPath = ConfigurationManager.AppSettings["AssetAppPath"] != null
                ? ConfigurationManager.AppSettings["AssetAppPath"]
                : "assets";

            staticResourcesVersion = ConfigurationManager.AppSettings["AssetsApp:StaticResourcesVersion"] != null
                ? ConfigurationManager.AppSettings["AssetsApp:StaticResourcesVersion"]
                : "assets";
        }

        public static string OldMainAppPath => oldMainAppPath;

        public static bool EnableKickOutWhenSessionInvalid => enableKickOutWhenSessionInvalid;

        public static string SessionVerificationTokenCookie => sessionVerificationTokenCookie;

        public static bool EnableSessionValidation => enableSessionValidation;

        public static bool EnableAntiXSRF => enableAntiXSRF;

        public static string MainAppPath => mainAppPath;

        public static string AssetAppPath => assetAppPath;

        public static string StaticResourcesVersion => staticResourcesVersion;
    }
}