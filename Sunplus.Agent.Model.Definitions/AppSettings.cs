namespace Sunplus.Agent.Model.BL.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;

    [Obsolete("Should rename to Sunplus.Agent.Model.Definitions")]
    public enum UserTypes : int
    {
        Admin = 5,
        Super = 4,
        Master = 3,
        Agent = 2,
        Member = 1,
        Unknown = 0
    }

    [Obsolete("Should rename to Sunplus.Agent.Model.Definitions")]
    public enum ServerIds : int
    {
        Unknown = -1,
        Agent = 90,
        Master = 91,
        Super = 92
    }

    [Obsolete("Should rename to Sunplus.Agent.Model.Definitions")]
    public struct SiteNames
    {
        public const string Super = "Super";
        public const string Master = "Master";
        public const string Agent = "Agent";
        public const string Unknown = "Unknown";
    }

    [Obsolete("Should rename to Sunplus.Agent.Model.Definitions")]
    public static class AppSettings
    {
        // Sub-account suffix
        public const string SUB_ACC_SUFFIX = "Sub";

        // Key for session validating service
        public const string SESSION_VALIDATING_KEY = "SESSION_VALIDATING";

        internal const char TwoDot = ':';
        internal const char DomainSeperator = ';';
        internal const string HttpSlashSlash = "http://";
        internal const string HttpSlashSlashS = "https://";
        internal const char UrlSlash = '/';

        private const int MAX = 1010;

        // Valid site name
        private static string[] validSiteNames = new string[] { SiteNames.Unknown, SiteNames.Unknown, SiteNames.Agent, SiteNames.Master, SiteNames.Super };

        private static Random ran = new Random();

        public static bool IsHideOnlineList
        {
            get
            {
                return GetValue("RemoveOnlineList") == "1";
            }
        }

        public static bool IsEnableP2P
        {
            get
            {
                return GetValue("EnableP2P") == "1";
            }
        }

        public static bool IsEnableLiveCasino
        {
            get
            {
                return GetValue("EnableLiveCasino") == "1";
            }
        }

        public static bool IsEnableKeno
        {
            get
            {
                return GetValue("EnableKeno") == "1";
            }
        }

        public static bool IsEnableHRFixedOdds
        {
            get
            {
                return GetValue("EnableHRFixedOdds") == "1";
            }
        }

        public static bool IsEnableBingo
        {
            get
            {
                return GetValue("EnableBingo") == "1";
            }
        }

        public static bool IsEnableMemberWinLimit
        {
            get
            {
                return GetValue("EnableMemberWinLimit") == "1";
            }
        }

        public static bool IsEnableDoubleCommission
        {
            get
            {
                return GetValue("EnableDoubleCommission") == "1";
            }
        }

        public static bool IsShowGoldDeluxeSettingForCredit
        {
            get
            {
                return GetValue("IsShowGoldDeluxeSettingForCredit") == "1";
            }
        }

        public static bool IsShowColossusBets
        {
            get
            {
                return GetValue("IsShowColossusBets") == "1";
            }
        }

        public static bool IsSupportMyanmarOdds
        {
            get
            {
                return GetValue("IsShowMyanmarOdds") == "1";
            }
        }

        public static bool IsShowSportsbook2
        {
            get
            {
                return GetValue("IsShowSportsbook2") == "1";
            }
        }

        #region Domain Section

        /// <summary>
        /// Get base url of current website with cookieless.
        /// </summary>
        public static string BaseUrl2
        {
            get
            {
                return GetBaseUrl2();
            }
        }

        /// <summary>
        /// Get base url of current website.
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                return GetBaseUrl();
            }
        }

        /// <summary>
        /// Before login domain.
        /// </summary>
        public static string BeforeLoginUrl
        {
            get
            {
                string baseUrl = GetBaseUrl();
                var domains = ConfigurationManager.AppSettings;

                foreach (string key in domains.AllKeys)
                {
                    if (domains[key].Contains(baseUrl))
                    {
                        return key;
                    }
                }

                return baseUrl;
            }
        }

        /// <summary>
        /// All after login domains
        /// </summary>
        public static string[] AfterLoginDomainCollection
        {
            get
            {
                string baseUrl = LocalTest ? GetBaseUrl() : BeforeLoginUrl;

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[baseUrl]))
                {
                    return new string[] { };
                }

                return ConfigurationManager.AppSettings[baseUrl].Split(new char[] { DomainSeperator }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// Get cookieless domains
        /// </summary>
        public static string[] CookielessDomainCollection
        {
            get
            {
                string[] domains = AfterLoginDomainCollection;
                if (domains.Length <= 0)
                {
                    return domains;
                }

                string[] cookielessDomains = new string[domains.Length - 1];
                int c = cookielessDomains.Length;
                for (int i = 0; i < c; i++)
                {
                    cookielessDomains[i] = domains[i + 1];
                }

                return cookielessDomains;
            }
        }

        /// <summary>
        /// Get after login domain.
        /// </summary>
        public static string AfterLoginUrl
        {
            get
            {
                return AfterLoginDomainCollection[0];
            }
        }

        public static string CookielessPath
        {
            get
            {
                var isCookieless = HttpContext.Current.Session.IsCookieless;
                var cookielessPath = isCookieless ? string.Format("(S({0}))/", HttpContext.Current.Session.SessionID) : string.Empty;

                return cookielessPath;
            }
        }

        #endregion Domain Section

        #region Configurations

        public static bool EnableSessionValidatingService
        {
            get
            {
                return int.Parse(GetValue("EnableSessionValidatingService")) != 0;
            }
        }

        /// <summary>
        /// Current super id of ISPO line.
        /// </summary>
        public static int ISPOSuperId
        {
            get
            {
                return int.Parse(GetValue("ISPOSuperId"));
            }
        }

        public static int SpecialSuperId
        {
            get
            {
                return int.Parse(GetValue("SpecialSuperId"));
            }
        }

        public static string ISPOUrl
        {
            get
            {
                return GetValue("ISPOUrl");
            }
        }

        /// <summary>
        /// Paging size.
        /// </summary>
        public static int PageSize
        {
            get
            {
                return int.Parse(GetValue("PageSize"));
            }
        }

        public static DateTime SB_HR_FI_Date
        {
            get
            {
                return DateTime.Parse(GetValue("SB_HR_FI_Date"));
            }
        }

        public static DateTime Min_Report_Date
        {
            get
            {
                return DateTime.Parse(GetValue("Min_Report_Date"));
            }
        }

        public static string LoggingServiceUrl
        {
            get
            {
                return GetValue("LoggingServiceUrl");
            }
        }

        public static string P2PServiceUrl
        {
            get
            {
                return GetValue("P2PServiceUrl");
            }
        }

        public static string P2PSiteId
        {
            get
            {
                return GetValue("P2PSiteId");
            }
        }

        public static string RemoteRefURL_Msg
        {
            get
            {
                return GetValue("RemoteRefURL_Msg");
            }
        }

        public static string RemoteRefURL
        {
            get
            {
                return GetValue("RemoteRefURL");
            }
        }

        public static string SiteUrl
        {
            get
            {
                return GetValue("SiteUrl");
            }
        }

        public static string JsVersion
        {
            get
            {
                return GetValue("JsVersion");
            }
        }

        public static string Site
        {
            get
            {
                return GetValue("Site");
            }
        }

        public static bool ShowLetter
        {
            get
            {
                return int.Parse(GetValue("ShowLetter")) != 0;
            }
        }

        public static string DefaultNoIPVN
        {
            get
            {
                return GetValue("DefaultNoIPVN");
            }
        }

        public static string GoogleMapKey
        {
            get
            {
                return GetValue("GoogleMapKey");
            }
        }

        /// <summary>
        /// List of special super id: allow on Total Balance transfer.
        /// </summary>
        public static string AllowedListSuperAccounts
        {
            get
            {
                return GetValue("AllowedListSuperAccounts") as string;
            }
        }

        public static bool VirtualDirectory
        {
            get
            {
                return int.Parse(GetValue("VirtualDirectory")) != 0;
            }
        }

        /// <summary>
        /// DisabledCasino in config.
        /// </summary>
        public static bool DisabledCasino
        {
            get
            {
                return int.Parse(GetValue("DisabledCasino")) != 0;
            }
        }

        /// <summary>
        /// DisabledAutoMaster in cofig.
        /// </summary>
        public static bool DisabledAutoMaster
        {
            get
            {
                return int.Parse(GetValue("DisabledAutoMaster")) != 0;
            }
        }

        #region Racing configurations

        /// <summary>
        /// Disable151_31 in config.
        /// </summary>
        public static bool Disable151_31
        {
            get
            {
                return int.Parse(GetValue("Disable151_31")) != 0;
            }
        }

        /// <summary>
        /// Disable151_41 in config.
        /// </summary>
        public static bool Disable151_41
        {
            get
            {
                return int.Parse(GetValue("Disable151_41")) != 0;
            }
        }

        /// <summary>
        /// Disable151_51 in config.
        /// </summary>
        public static bool Disable151_51
        {
            get
            {
                return int.Parse(GetValue("Disable151_51")) != 0;
            }
        }

        /// <summary>
        /// Disable152_41 in config.
        /// </summary>
        public static bool Disable152_41
        {
            get
            {
                return int.Parse(GetValue("Disable152_41")) != 0;
            }
        }

        /// <summary>
        /// Disable152_51 in config.
        /// </summary>
        public static bool Disable152_51
        {
            get
            {
                return int.Parse(GetValue("Disable152_51")) != 0;
            }
        }

        /// <summary>
        /// Disable153_51 in config.
        /// </summary>
        public static bool Disable153_51
        {
            get
            {
                return int.Parse(GetValue("Disable153_51")) != 0;
            }
        }

        #endregion Racing configurations

        /// <summary>
        /// Is local test.
        /// </summary>
        public static bool LocalTest
        {
            get
            {
                return int.Parse(GetValue("LocalTest")) != 0;
            }
        }

        /// <summary>
        /// Virtual application path.
        /// </summary>
        public static string VirtualPath
        {
            get
            {
                return GetValue("VirPath");
            }
        }

        /// <summary>
        /// First use for maintenance service.
        /// </summary>
        public static string LBUrl
        {
            get
            {
                return GetValue("LBUrl");
            }
        }

        /// <summary>
        /// Show View log function
        /// </summary>
        public static bool ShowViewLog
        {
            get
            {
                return int.Parse(GetValue("ShowViewLog")) != 0;
            }
        }

        public static string UseArchive
        {
            get
            {
                return GetValue("UseArchive");
            }
        }

        public static bool IsISPOSite
        {
            get
            {
                char splitChar = DomainSeperator;
                string[] ispoUrl = ISPOUrl.Split(splitChar);
                int c = ispoUrl.Length;
                string url = HttpContext.Current.Request.Url.OriginalString;
                for (int i = 0; i < c; i++)
                {
                    if (url.Contains(ispoUrl[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Neutral site, remove all things related to "IBC" for end user.
        /// </summary>
        public static bool IsNeutralSite
        {
            get
            {
                return int.Parse(GetValue("IsNeutralSite")) == 1;
            }
        }

        /// <summary>
        /// Show or Hide all functions of Number Game Only
        /// </summary>
        ///
        public static bool DisabledNumberGame
        {
            get
            {
                return int.Parse(GetValue("Disabled_NumberGame")) == 1;
            }
        }

        /// <summary>
        /// Show or Hide Number Group bettype in Number Game
        /// </summary>
        public static bool EnableNumberGroup
        {
            get
            {
                return ConfigurationManager.AppSettings["EnableNumberGroup"] == "1";
            }
        }

        /// <summary>
        /// Show or Hide Commssion Number Game
        /// </summary>
        public static bool DisabledNumberGameCommission
        {
            get
            {
                return int.Parse(GetValue("Disabled_NumberCommission")) == 1;
            }
        }

        /// <summary>
        /// Show or Hide Commssion Bingo Game
        /// </summary>
        public static bool DisabledBingoCommission
        {
            get
            {
                return int.Parse(GetValue("Disabled_BingoCommission")) == 1;
            }
        }

        /// <summary>
        /// Hide or Show Max Transfer Bingo Only
        /// </summary>
        public static bool DisabledBingoMaxTransfer
        {
            get
            {
                return int.Parse(GetValue("Disabled_BingoMaxTransfer")) == 1;
            }
        }

        /// <summary>
        /// Disabled and Enabled Bingo and Number Game status
        /// </summary>
        public static bool DisableBingoStatus
        {
            get
            {
                return int.Parse(GetValue("Disabled_BingoStatus")) == 1;
            }
        }

        /// <summary>
        /// Disabled and Enabled Number Game status
        /// </summary>
        public static bool DisableNumberGameStatus
        {
            get
            {
                return int.Parse(GetValue("Disabled_NumberGameStatus")) == 1;
            }
        }

        /// <summary>
        /// Show and Hide Parlay Mixed Sports
        /// </summary>
        public static bool IsShowMixedSport
        {
            get
            {
                return int.Parse(GetValue("IsShowMixedSport")) == 1;
            }
        }

        /// <summary>
        /// List of sites need to check security verification
        /// </summary>
        private static List<string> _enableSecurityVerify = null;

        public static List<string> EnableSecurityVerify
        {
            get
            {
                if (_enableSecurityVerify == null)
                {
                    string sites = (GetValue("EnableSecurityVerify") + string.Empty).ToLower();
                    _enableSecurityVerify = new List<string>(sites.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
                }

                return _enableSecurityVerify;
            }
        }

        /// <summary>
        /// Get value from a setting key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #region Setting functions

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static string SiteNameFromRoleId(int roleId)
        {
            if (roleId > 4)
            {
                return null;
            }

            return validSiteNames[roleId].ToString();
        }

        public static ServerIds ServerIdFromRoleId(int roleId)
        {
            switch (roleId)
            {
                case 2: return ServerIds.Agent;
                case 3: return ServerIds.Master;
                case 4: return ServerIds.Super;
            }

            return ServerIds.Unknown;
        }

        #endregion Setting functions

        private static int GetRandomNumber(int length)
        {
            if (length <= 1)
            {
                return 0;
            }

            int num1 = ran.Next(0, MAX);
            return num1 % length;
        }

        /// <summary>
        /// Get base url of current website.
        /// </summary>
        /// <returns></returns>
        private static string GetBaseUrl()
        {
            string hostName = HttpContext.Current.Request.Headers["Host"];
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string url = string.Format("//{0}{1}", hostName, appPath);

            if (appPath.Length > 1)
            {
                url += UrlSlash;
            }

            return url;
        }

        /// <summary>
        /// Get base url of current website with cookieless.
        /// </summary>
        /// <returns></returns>
        private static string GetBaseUrl2()
        {
            string hostName = HttpContext.Current.Request.Headers["Host"];
            string appPath = HttpContext.Current.Response.ApplyAppPathModifier(HttpContext.Current.Request.ApplicationPath);
            if (appPath[appPath.Length - 1] != UrlSlash)
            {
                appPath += UrlSlash;
            }

            return (HttpContext.Current.Request.IsSecureConnection ? HttpSlashSlashS : HttpSlashSlash) + hostName + appPath;
        }

        public static string MonitoringBetListBettypeOutrightList
        {
            get
            {
                return GetValue("BetListMonitoring_BettypeOutrightList");
            }
        }

        public static string MonitoringBetListBettypeVirtualSportsOutrightList
        {
            get
            {
                return GetValue("BetListMonitoring_BettypeVSOutrightList");
            }
        }

        public static string SubAccountPermission_LockAdminAccounts
        {
            get
            {
                return GetValue("SubAccountPermission_LockAdminAccounts");
            }
        }

        /// <summary>
        /// FPS: link to call FPS.
        /// </summary>
        public static string FPSApi
        {
            get
            {
                return GetValue("FPS_API");
            }
        }

        // Piwik
        public static string PiwikUrl
        {
            get
            {
                return GetValue("PiwikUrl");
            }
        }

        public static string PiwikScript
        {
            get
            {
                return GetValue("PiwikScript");
            }
        }

        public static string PiwikSiteId
        {
            get
            {
                return GetValue("PiwikSiteId");
            }
        }

        public static bool PiwikEnable
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PiwikSiteId);
            }
        }

        #endregion Configurations

        public static bool IsEnabledProduct(int product)
        {
            return GetValue("EnableProduct-" + product) == "1";
        }
    }
}