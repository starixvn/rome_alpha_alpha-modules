namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities.Base;

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "Reviewed.")]
    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public interface IUserSession : IEntity, ISerializable
    {
        #region Misc

        /// <summary>
        /// Default: en-US
        /// </summary>
        string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Values: E,J,CT,CS,TH,KO.
        /// </summary>
        string LangKey
        {
            get;
            set;
        }

        CultureInfo Culture
        {
            get;
        }

        /// <summary>
        /// Current captchar in login page.
        /// </summary>
        string CaptchaCode
        {
            get;
            set;
        }

        /// <summary>
        /// True that mean current currency is in set: RM,SD$,RMB,HK$,US$,EURO,TB,THB,IDD,IDR,IN,IND.
        /// </summary>
        bool IsBelongTo9Currency
        {
            get;
            set;
        }

        #endregion Misc

        #region Logged-in informations

        /// <summary>
        /// Id of admin account (logging from accounting site).
        /// </summary>
        int AdminId
        {
            get;
        }

        /// <summary>
        /// User name of admin account (logging from accounting site).
        /// </summary>
        string AdminName
        {
            get;
        }

        /// <summary>
        /// Id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        int CustId
        {
            get;
        }

        /// <summary>
        /// Role id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        byte RoleId
        {
            get;
        }

        /// <summary>
        /// Server id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        int ServerId
        {
            get;
        }

        /// <summary>
        /// User name of main logged-in account.
        /// </summary>
        string UserName
        {
            get;
        }

        /// <summary>
        /// Nick name of main logged-in account.
        /// </summary>
        string NickName
        {
            get;
            set;
        }

        /// <summary>
        /// DisplayNickname of main logged-in account.
        /// </summary>
        string DisplayNickName
        {
            get;
            set;
        }

        /// <summary>
        /// Return 0 if it is not available.
        /// </summary>
        bool IsNicknameLoginOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Return 0 if it is not available.
        /// </summary>
        bool IsNickname
        {
            get;
            set;
        }

        /// <summary>
        /// Logged-in indentify on OnUser database, use for ReCheckin and Kichout feature.
        /// Return 0 if it is not available.
        /// </summary>
        int OnUserId
        {
            get;
        }

        /// <summary>
        /// Id of logged-in sub-account.
        /// If logged-in account is main, return 0.
        /// </summary>
        int SubAccId
        {
            get;
        }

        /// <summary>
        /// True if logged-in is sub-account.
        /// </summary>
        bool IsSubAcc
        {
            get;
        }

        /// <summary>
        /// True if logged-in is administrator.
        /// </summary>
        bool IsInternal
        {
            get;
        }

        /// <summary>
        /// 12bet,789y,indo...
        /// </summary>
        string Site
        {
            get;
        }

        /// <summary>
        /// Logged-in sub-account access.
        /// If logged-in account is main, return null.
        /// </summary>
        PageAccess SubPageAccess
        {
            get;
        }

        /// <summary>
        /// User name of logged-in sub-account.
        /// If logged-in account is main, return null.
        /// </summary>
        string SubUserName
        {
            get;
        }

        /// <summary>
        /// Current site name: Agent, Master, Super.
        /// </summary>
        string SiteName
        {
            get;
        }

        /// <summary>
        /// User is using system: AdminId if logged-in user is administrator, otherwise is CustId.
        /// </summary>
        int UserId
        {
            get;
        }

        /// <summary>
        /// Current currency name.
        /// </summary>
        string Currency
        {
            get;
        }

        /// <summary>
        /// Current currency id.
        /// </summary>
        int CurrencyId
        {
            get;
        }

        /// <summary>
        /// False that mean first logged-in time.
        /// </summary>
        bool IsLogged
        {
            get;
            set;
        }

        /// <summary>
        /// Indicate that the password was expired
        /// </summary>
        bool IsPasswordExpired
        {
            get;
            set;
        }

        /// <summary>
        /// Indicate that the last time pwd was changed
        /// </summary>
        DateTime LastChangedPassword
        {
            get;
            set;
        }

        /// <summary>
        /// True indicate admin user can view VN ip.
        /// </summary>
        bool IsAdminCanViewVNIP
        {
            get;
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        bool IsAgent
        {
            get;
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        bool IsMaster
        {
            get;
        }

        /// <summary>
        /// True indicate current user is super
        /// </summary>
        bool IsSuper
        {
            get;
        }

        /// <summary>
        /// True indicate current user is group header
        /// </summary>
        bool IsShowConsolidatedReport
        {
            get;
            set;
        }

        // Security code
        bool IsForcedSecurityCode { get; set; }

        /// <summary>
        /// Security code of current user
        /// </summary>
        string SecurityCode { get; set; }

        SCResult SecurityCodeResult { get; set; }

        bool ShowSecurityCodePopup { get; set; }

        string SecurityCodeToken { get; set; }

        bool IsEnabledSecurityCodeInBetLists { get; set; }

        bool IsEnabledSecurityCodeInStatement { get; set; }

        #endregion Logged-in informations

        #region Casino

        /// <summary>
        /// Disabled casino status of logged-in user.
        /// </summary>
        bool IsCasinoDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Synchronized casino status of logged-in user.
        /// </summary>
        bool IsCasinoSync
        {
            get;
            set;
        }

        /// <summary>
        /// True that mean current currency is casino currency.
        /// </summary>
        bool IsNotCurrencyCasino
        {
            get;
            set;
        }

        #endregion Casino       

        #region VirtualSports

        bool IsHideVirtualSports
        {
            get;
            set;
        }

        bool IsDisabledVirtualSports
        {
            get;
            set;
        }

        #endregion VirtualSports        

        #region Keno

        bool IsHideKeno
        {
            get;
            set;
        }

        #endregion Keno

        #region GoldDeluxe

        bool IsHideGoldDeluxe { get; set; }

        bool DisableGoldDeluxe { get; set; }

        #endregion GoldDeluxe

        #region Sportsbook2

        bool IsHideSportsbook2 { get; set; }

        bool DisableSportsbook2 { get; set; }

        #endregion Sportsbook2

        #region Double Commission

        bool IsDoubleCommLosing
        {
            get;
            set;
        }

        #endregion Double Commission

        /// <summary>
        /// Disabled Number Game status of logged-in user.
        /// </summary>
        bool IsNumberGameDisabled
        {
            get;
        }       

        #region PostionTaking

        bool IsDisabledAutoMPT
        {
            get;
        }

        #endregion PostionTaking

        #region UplineInformations

        /// <summary>
        /// Super Id. Return 0 if it is not available.
        /// </summary>
        int SRecommendId
        {
            get;
        }

        /// <summary>
        /// Master Id. Return 0 if it is not available.
        /// </summary>
        int MRecommendId
        {
            get;
        }

        /// <summary>
        /// Agent Id. Return 0 if it is not available.
        /// </summary>
        int ARecommendId
        {
            get;
        }

        #endregion UplineInformations

        #region TotalBalanceTransfer

        /// <summary>
        /// Under supers which allow transfer on Total Balance.
        /// </summary>
        bool IsAllowedTotalBalanceTransfer
        {
            get;
        }

        #endregion TotalBalanceTransfer

        /// <summary>
        /// Current session id.
        /// </summary>
        string NetSessionId
        {
            get;
        }

        #region

        bool IsLimitedHostPermisionOnSubAccountFunction
        {
            get;
            set;
        }

        #endregion

        #region Show/Hide Menu

        bool IsShowBetChoiceLimit { get; set; }

        #endregion

        bool IsDisabledESports
        {
            get;
            set;
        }

        bool IsMyanmarOdds { get; set; }

        bool IsDisabledSuperLive { get; set; }

        bool GetHideProduct(int productId);

        void SetHideProduct(int productId, bool value);

        bool GetSyncedProduct(int productId);

        void SetSyncedProduct(int productId, bool value);

        bool GetDisabledProduct(int productId);

        void SetDisabledProduct(int productId, bool value);

        string UserLoginIp { get; set; }

        bool IsInternalWebVPN
        {
            get;
            set;
        }

        bool IsAllDayBalTransfer { get; }

        bool IsShowSkipForceChangePassword { get; set; }

        bool IsIndirectLoggedIn { get; }

        int IndirectCustId { get; set; }

        int IndirectSubAccId { get; set; }

        string IndirectUsername { get; set; }

        string DarkModeName { get; set; }

        string AssetAppPath { get; set; }

        bool IsOfferProtectedMode { get; set; }

        bool IsProtectedMode { get; set; }

        T GetProperty<T>(string propertyName);

        void SetProperty(string propertyName, object value);
    }
}