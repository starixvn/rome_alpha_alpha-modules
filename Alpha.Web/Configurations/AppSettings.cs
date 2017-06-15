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

        private static string reportsAppPath;
        private static string betListsAppPath;
        private static string memberInfoAppPath;

        private static string staticResourcesVersion;
        private static string googleAnalyticUrl;
        private static string googleAnalyticId;

        private static bool isIntegrationTest;

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

            oldMainAppPath = ConfigurationManager.AppSettings["OldMainAppPath"] ?? "ex-main";

            mainAppPath = ConfigurationManager.AppSettings["MainAppPath"] ?? "site-main";

            assetAppPath = ConfigurationManager.AppSettings["AssetAppPath"] ?? "assets";

            reportsAppPath = ConfigurationManager.AppSettings["ReportsAppPath"] ?? "site-reports";
            betListsAppPath = ConfigurationManager.AppSettings["BetListsAppPath"] ?? "site-betlists";
            memberInfoAppPath = ConfigurationManager.AppSettings["MemberInfoAppPath"] ?? "site-memberinfo";

            staticResourcesVersion = ConfigurationManager.AppSettings["AssetsApp:StaticResourcesVersion"] ?? "100";

            googleAnalyticUrl = ConfigurationManager.AppSettings["GoogleAnalyticUrl"] ?? "https://www.google-analytics.com/analytics.js";

            googleAnalyticId = ConfigurationManager.AppSettings["GoogleAnalyticId"];

            isIntegrationTest = ConfigurationManager.AppSettings["IsIntegrationTest"] == "1";
        }

        public static string OldMainAppPath => oldMainAppPath;

        public static bool EnableKickOutWhenSessionInvalid => enableKickOutWhenSessionInvalid;

        public static string SessionVerificationTokenCookie => sessionVerificationTokenCookie;

        public static bool EnableSessionValidation => enableSessionValidation;

        public static bool EnableAntiXSRF => enableAntiXSRF;

        public static string MainAppPath => mainAppPath;

        public static string AssetAppPath => assetAppPath;

        public static string ReportsAppPath => reportsAppPath;

        public static string BetListsAppPath => betListsAppPath;

        public static string MemberInfoAppPath => memberInfoAppPath;

        public static string StaticResourcesVersion => staticResourcesVersion;

        public static string GoogleAnalyticUrl => googleAnalyticUrl;

        public static string GoogleAnalyticId => googleAnalyticId;

        public static bool IsIntegrationTest => isIntegrationTest;
    }
}
