namespace Alpha.Web.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

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
        private static string messageAppPath;
        private static string accountInfoAppPath;

        private static string staticResourcesVersion;
        private static string googleAnalyticUrl;
        private static string googleAnalyticId;

        private static bool isIntegrationTest;

        private static string exTotalBetsForecastAppPath;
        private static string howToUseAppPath;
        private static string viewLogsAppPath;
        private static string transferAppPath;

        private static bool updateCreditReplicateServer;
        private static int insertProfilePictureSiteMode;
        private static string[] hideProductStatusSites;

        private static string productConfigsFileRelativePath;
        private static string qrCodeMemberSiteLoginConfigsPath;

        private static DateTime? forceLoginNicknameDate;
        private static IDictionary<int, bool> forceLoginNicknameIgnoreList;
        private static DateTime? forceLoginNicknameDate4SubAccount;

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
            messageAppPath = ConfigurationManager.AppSettings["MessageAppPath"] ?? "site-message";
            accountInfoAppPath = ConfigurationManager.AppSettings["AccountInfoAppPath"] ?? "site-accountinfo";

            staticResourcesVersion = ConfigurationManager.AppSettings["AssetsApp:StaticResourcesVersion"] ?? "100";

            googleAnalyticUrl = ConfigurationManager.AppSettings["GoogleAnalyticUrl"] ?? "https://www.google-analytics.com/analytics.js";

            googleAnalyticId = ConfigurationManager.AppSettings["GoogleAnalyticId"];

            isIntegrationTest = ConfigurationManager.AppSettings["IsIntegrationTest"] == "1";

            exTotalBetsForecastAppPath = ConfigurationManager.AppSettings["ExTotalBetsForecastAppPath"] ?? "ex-totalbetsforecast";
            howToUseAppPath = ConfigurationManager.AppSettings["HowToUseAppPath"] ?? "site-howtouse";
            transferAppPath = ConfigurationManager.AppSettings["TransferAppPath"] ?? "site-transfer";
            viewLogsAppPath = ConfigurationManager.AppSettings["ViewLogsAppPath"] ?? "site-viewLogs";

            updateCreditReplicateServer = ConfigurationManager.AppSettings["UpdateCreditReplicateServer"] == "1";
            insertProfilePictureSiteMode = Fanex.Common.Converter.ToInt(ConfigurationManager.AppSettings["InsertProfilePictureSiteMode"]);

            hideProductStatusSites = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["HideProductStatus:Sites"]) ?
            new string[] { } : ConfigurationManager.AppSettings["HideProductStatus:Sites"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            productConfigsFileRelativePath = ConfigurationManager.AppSettings["ProductConfigsFileRelativePath"];
            qrCodeMemberSiteLoginConfigsPath = ConfigurationManager.AppSettings["QRCodeMemberSiteLoginConfigs"];

            GetForceLoginNicknameConfiguration();
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

        public static string MessageAppPath => messageAppPath;

        public static string AccountInfoAppPath => accountInfoAppPath;

        public static string StaticResourcesVersion => staticResourcesVersion;

        public static string GoogleAnalyticUrl => googleAnalyticUrl;

        public static string GoogleAnalyticId => googleAnalyticId;

        public static bool IsIntegrationTest => isIntegrationTest;

        public static string ExTotalBetsForecastAppPath => exTotalBetsForecastAppPath;

        public static string HowToUseAppPath => howToUseAppPath;

        public static string TransferAppPath => transferAppPath;
        
        public static string ViewLogsAppPath => viewLogsAppPath;

        public static DateTime? ForceLoginNicknameDate => forceLoginNicknameDate;

        public static IDictionary<int, bool> ForceLoginNicknameIgnoreList => forceLoginNicknameIgnoreList;

        public static DateTime? ForceLoginNicknameDate4SubAccount => forceLoginNicknameDate4SubAccount;

        public static bool UpdateCreditBothDBCustAndDBAge => updateCreditReplicateServer;

        public static int InsertProfilePictureSiteMode => insertProfilePictureSiteMode;

        public static string ProductConfigsFileRelativePath => productConfigsFileRelativePath;

        public static string QRCodeMemberSiteLoginConfigsPath => qrCodeMemberSiteLoginConfigsPath;

        public static string[] HideProductStatusSites => hideProductStatusSites;

        private static void GetForceLoginNicknameConfiguration()
        {
            var forceLoginNicknameDateValue = ConfigurationManager.AppSettings["ForceLoginNicknameDate"];
            forceLoginNicknameDate = string.IsNullOrWhiteSpace(forceLoginNicknameDateValue)
                ? default(DateTime?)
                : DateTime.ParseExact(forceLoginNicknameDateValue, "dd/MM/yyyy", null);

            var forceLoginNicknameIgnoreListValue = ConfigurationManager.AppSettings["ForceLoginNicknameIgnoreList"];
            forceLoginNicknameIgnoreList = string.IsNullOrWhiteSpace(forceLoginNicknameIgnoreListValue)
                ? new Dictionary<int, bool>()
                : forceLoginNicknameIgnoreListValue
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(kvp => int.Parse(kvp), kvp => true);

            var forceLoginNicknameDate4SubAccountValue = ConfigurationManager.AppSettings["ForceLoginNicknameDate4SubAccount"];
            forceLoginNicknameDate4SubAccount = string.IsNullOrWhiteSpace(forceLoginNicknameDate4SubAccountValue)
                ? default(DateTime?)
                : DateTime.ParseExact(forceLoginNicknameDate4SubAccountValue, "dd/MM/yyyy", null);
        }
    }
}
