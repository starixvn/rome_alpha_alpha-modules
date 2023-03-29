namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Web;
    using Sunplus.Agent.Model.BL.Definitions;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [Serializable]
    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class UserSessionBase : IUserSession, IDisposable
    {
        #region Contructors

        public const string DEFAULT_LANGUAGE = "en-US";
        private const string HYPHEN = "_";
        private const string ISBELONGTO9CURRENCY = "ISBELONGTO9CURRENCY";

        /// <summary>
        /// Values: E,J,CT,CS,TH,KO.
        /// </summary>
        private const string LANGKEY = "LANGKEY";

        private const string CAPTCHACODE = "CAPTCHACODE";
        private const string ADMINID = "ADMINID";
        private const string ADMINNAME = "ADMINNAME";
        private const string CUSTID = "CUSTID";
        private const string ROLEID = "ROLEID";
        private const string SERVERID = "SERVERID";
        private const string USERNAME = "USERNAME";
        private const string NICKNAME = "NICKNAME";
        private const string DISPLAYNICKNAME = "DISPLAYNICKNAME";
        private const string ONUSERID = "ONUSERID";
        private const string SUBACCID = "SUBACCID";
        private const string SUBPAGEACCESS = "SUBPAGEACCESS";
        private const string SUBUSERNAME = "SUBUSERNAME";
        private const string SITE = "SITE";
        private const string SITENAME = "SITENAME";
        private const string CURRENCY = "CURRENCY";
        private const string CURRENCYID = "CURRENCYID";
        private const string ISLOGGED = "ISLOGGED";

        private const string ISPASSWORDEXPIRED = "ISPASSWORDEXPIRED";
        private const string LASTCHANGEDPASSWORD = "LASTCHANGEDPASSWORD";

        // Security code
        private const string ISFORCEDSECURITYCODE = "ISFORCEDSECURITYCODE";

        private const string SECURITYCODE = "SECURITYCODE";
        private const string SECURITYCODERESULT = "SECURITYCODERESULT";
        private const string SHOWSECURITYCODEPOPUP = "SHOWSECURITYCODEPOPUP";
        private const string SECURITYCODETOKEN = "SECURITYCODETOKEN";
        private const string ISENABLEDSECURITYCODEINBETLISTS = "ISENABLEDSECURITYCODEINBETLISTS";
        private const string ISENABLEDSECURITYCODEINSTATEMENT = "ISENABLEDSECURITYCODEINSTATEMENT";

        private const string ISNICKNAMELOGINONLY = "ISNICKNAMELOGINONLY";
        private const string ISADMINCANVIEWVNIP = "ISADMINCANVIEWVNIP";
        private const string ISNICKNAME = "ISNICKNAME";
        private const string ISSHOWCONSOLIDATEDREPORT = "ISSHOWCONSOLIDATEDREPORT";
        private const string ISSHOWBETCHOICELIMIT = "ISSHOWBETCHOICELIMIT";      
        private const string ISNUMBERGAMEDISABLED = "ISNUMBERGAMEDISABLED";

        private const string ISDOUBLECOMMLOSING = "DOUBLECOMMLOSING";

        private const string ISHIDEVIRTUALSPORTS = "ISHIDEVIRTUALSPORTS";
        private const string ISDISABLEDVIRTUALSPORTS = "ISDISABLEDVIRTUALSPORTS";
        private const string ISDISABLEDAUTOMPT = "ISDISABLEDAUTOMPT";

        private const string SRECOMMENDID = "SRECOMMENDID";
        private const string MRECOMMENDID = "MRECOMMENDID";
        private const string ARECOMMENDID = "ARECOMMENDID";

        private const string ISALLOWEDTOTALBALANCETRANSFER = "ISALLOWEDTOTALBALANCETRANSFER";

        private const string LANGUAGE = "LANGUAGE";

        private const string ISHIDEKENO = "ISHIDEKENO";

        private const string ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION = "ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION";

        private const string ISDISABLEDESPORTS = "ISDISABLEDESPORTS";
        private const string ISMYANMARODDS = "ISMYANMARODDS";
        private const string ISHIDESPORTSBOOK2 = "ISHIDESPORTSBOOK2";
        private const string DISABLESPORTSBOOK2 = "DISABLESPORTSBOOK2";

        private const string DISABLESUPERLIVE = "DISABLESUPERLIVE";

        private const string HIDEPRODUCT = "HIDEPRODUCT";
        private const string DISABLEDPRODUCT = "DISABLEDPRODUCT";
        private const string SYNCEDPRODUCT = "SYNCEDPRODUCT";

        private const string USERLOGINIP = "USERLOGINIP";

        private const string ISSHOWSKIPFORCECHANGPASSWORD = "ISSHOWSKIPFORCECHANGPASSWORD";
        private string asNetSessionId;

        private const string IndirectCustIdSessionKey = "IndirectCustId";
        private const string IndirectSubAccIdSessionKey = "IndirectSubAccId";
        private const string IndirectUsernameSessionKey = "IndirectUsername";

        private const string DARKMODENAMESESSIONKEY = "DARKMODENAME";
        private const string ASSETAPPPATHSESSIONKEY = "ASSETAPPPATH";

        private const string ISOFFERPROTECTEDMODESESSIONKEY = "ISOFFERPROTECTEDMODE";
        private const string ISPROTECTEDMODESESSIONKEY = "ISPROTECTEDMODE";

        public const string AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY = "AUTOFOLLOWBETISDUMMYAGENT";

        #endregion Contructors

        #region Misc

        public UserSessionBase(SerializationInfo info, StreamingContext context)
        {
            Type type = GetType();
            FieldInfo[] f = type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);

            int c = f.Length;
            for (int i = 0; i < c; i++)
            {
                f[i].SetValue(this, info.GetValue(f[i].Name, f[i].FieldType));
            }
        }

        public UserSessionBase(string netSessionId)
        {
            this.asNetSessionId = netSessionId;
        }

        public enum UserTypes : int
        {
            Admin = 5,
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1,
            Unknown = 0
        }

        public string Language
        {
            get
            {
                object value = this.GetProperty(LANGUAGE);
                return (null == value) ? DEFAULT_LANGUAGE : value as string;
            }

            set
            {
                // Temporary cheat for old page work
                HttpContext.Current.Session["Tpl_Culture"] = value;

                this.SetProperty(LANGUAGE, value);
            }
        }

        public string LangKey
        {
            get
            {
                object value = this.GetProperty(LANGKEY);
                return (null == value) ? "E" : value as string;
            }

            set
            {
                this.SetProperty(LANGKEY, value);
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return new CultureInfo(this.Language);
            }
        }

        /// <summary>
        /// Current captchar in login page.
        /// </summary>
        public string CaptchaCode
        {
            get
            {
                object value = this.GetProperty(CAPTCHACODE);
                return value as string;
            }

            set
            {
                this.SetProperty(CAPTCHACODE, value);
            }
        }

        /// <summary>
        /// True that mean current currency is in set: RM,SD$,RMB,HK$,US$,EURO,TB,THB,IDD,IDR,IN,IND.
        /// </summary>
        public bool IsBelongTo9Currency
        {
            get
            {
                object value = this.GetProperty(ISBELONGTO9CURRENCY);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISBELONGTO9CURRENCY, value);
            }
        }

        #endregion Misc

        #region Logged-in informations

        /// <summary>
        /// Id of admin account (logging from accounting site).
        /// </summary>
        public int AdminId
        {
            get
            {
                object value = this.GetProperty(ADMINID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(ADMINID, value);
            }
        }

        /// <summary>
        /// User name of admin account (logging from accounting site).
        /// </summary>
        public string AdminName
        {
            get
            {
                object value = this.GetProperty(ADMINNAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(ADMINNAME, value);
            }
        }

        /// <summary>
        /// Id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        public int CustId
        {
            get
            {
                object value = this.GetProperty(CUSTID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(CUSTID, value);
            }
        }

        /// <summary>
        /// Role id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        public byte RoleId
        {
            get
            {
                object value = this.GetProperty(ROLEID);
                return (null == value) ? byte.MinValue : (byte)value;
            }

            set
            {
                this.SetProperty(ROLEID, value);
            }
        }

        /// <summary>
        /// Server id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        public int ServerId
        {
            get
            {
                object value = this.GetProperty(SERVERID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(SERVERID, value);
            }
        }

        /// <summary>
        /// User name of main logged-in account.
        /// </summary>
        public string UserName
        {
            get
            {
                object value = this.GetProperty(USERNAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(USERNAME, value);
            }
        }

        /// <summary>
        /// Nick name of main logged-in account.
        /// </summary>
        public string NickName
        {
            get
            {
                object value = this.GetProperty(NICKNAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(NICKNAME, value);
            }
        }

        /// <summary>
        /// Display Nickname of main logged-in account.
        /// </summary>
        public string DisplayNickName
        {
            get
            {
                object value = this.GetProperty(DISPLAYNICKNAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(DISPLAYNICKNAME, value);
            }
        }

        /// <summary>
        /// Logged-in indentify on OnUser database, use for ReCheckin and Kichout feature.
        /// Return 0 if it is not available.
        /// </summary>
        public int OnUserId
        {
            get
            {
                object value = this.GetProperty(ONUSERID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(ONUSERID, value);
            }
        }

        /// <summary>
        /// Id of logged-in sub-account.
        /// If logged-in account is main, return 0.
        /// </summary>
        public int SubAccId
        {
            get
            {
                object value = this.GetProperty(SUBACCID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(SUBACCID, value);
            }
        }

        /// <summary>
        /// True if logged-in is sub-account.
        /// </summary>
        public bool IsSubAcc
        {
            get
            {
                return this.SubAccId != 0;
            }
        }

        /// <summary>
        /// True if logged-in is administrator.
        /// </summary>
        public bool IsInternal
        {
            get
            {
                return this.AdminId != 0;
            }
        }

        /// <summary>
        /// Logged-in sub-account access.
        /// If logged-in account is main, return empty object.
        /// </summary>
        public PageAccess SubPageAccess
        {
            get
            {
                PageAccess value = this.GetProperty(SUBPAGEACCESS) as PageAccess;
                return value ?? new PageAccess();
            }

            set
            {
                this.SetProperty(SUBPAGEACCESS, value);
            }
        }

        /// <summary>
        /// User name of logged-in sub-account.
        /// If logged-in account is main, return null.
        /// </summary>
        public string SubUserName
        {
            get
            {
                object value = this.GetProperty(SUBUSERNAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(SUBUSERNAME, value);
            }
        }

        /// <summary>
        /// 12bet,789y,indo...
        /// </summary>
        public string Site
        {
            get
            {
                object value = this.GetProperty(SITE);
                return value as string;
            }

            set
            {
                this.SetProperty(SITE, value);
            }
        }

        /// <summary>
        /// Current site name: Agent, Master, Super.
        /// </summary>
        public string SiteName
        {
            get
            {
                object value = this.GetProperty(SITENAME);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(SITENAME, value);
            }
        }

        /// <summary>
        /// User are using system.
        /// </summary>
        public int UserId
        {
            get
            {
                if (this.IsInternal)
                {
                    return this.AdminId;
                }
                else
                {
                    return this.CustId;
                }
            }
        }

        /// <summary>
        /// Current currency name.
        /// </summary>
        public string Currency
        {
            get
            {
                object value = this.GetProperty(CURRENCY);
                return (null == value) ? null : value as string;
            }

            set
            {
                this.SetProperty(CURRENCY, value);
            }
        }

        /// <summary>
        /// Current currency id.
        /// </summary>
        public int CurrencyId
        {
            get
            {
                object value = this.GetProperty(CURRENCYID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(CURRENCYID, value);
            }
        }

        /// <summary>
        /// False that mean first logged-in time.
        /// </summary>
        public bool IsLogged
        {
            get
            {
                object value = this.GetProperty(ISLOGGED);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISLOGGED, value);
            }
        }

        /// <summary>
        /// Indicate that the password was expired
        /// </summary>
        public bool IsPasswordExpired
        {
            get
            {
                object value = this.GetProperty(ISPASSWORDEXPIRED);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISPASSWORDEXPIRED, value);
            }
        }

        /// <summary>
        /// Indicate that the last time pwd was changed
        /// </summary>
        public DateTime LastChangedPassword
        {
            get
            {
                object value = this.GetProperty(LASTCHANGEDPASSWORD);
                return (null == value) ? DateTime.MinValue : (DateTime)value;
            }

            set
            {
                this.SetProperty(LASTCHANGEDPASSWORD, value);
            }
        }

        /// <summary>
        /// True indicate admin user can view VN ip
        /// </summary>
        public bool IsAdminCanViewVNIP
        {
            get
            {
                object value = this.GetProperty(ISADMINCANVIEWVNIP);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISADMINCANVIEWVNIP, value);
            }
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        public bool IsAgent
        {
            get
            {
                return this.RoleId == (int)UserTypes.Agent;
            }
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        public bool IsMaster
        {
            get
            {
                return this.RoleId == (int)UserTypes.Master;
            }
        }

        /// <summary>
        /// True indicate current user is super
        /// </summary>
        public bool IsSuper
        {
            get
            {
                return this.RoleId == (int)UserTypes.Super;
            }
        }

        /// <summary>
        /// True indicate current user login only nick name
        /// </summary>
        public bool IsNicknameLoginOnly
        {
            get
            {
                object value = this.GetProperty(ISNICKNAMELOGINONLY);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISNICKNAMELOGINONLY, value);
            }
        }

        /// <summary>
        /// True indicate current user login only nick name
        /// </summary>
        public bool IsNickname
        {
            get
            {
                object value = this.GetProperty(ISNICKNAME);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISNICKNAME, value);
            }
        }

        public bool IsShowConsolidatedReport
        {
            get
            {
                object value = this.GetProperty(ISSHOWCONSOLIDATEDREPORT);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISSHOWCONSOLIDATEDREPORT, value);
            }
        }

        // Security code
        public bool IsForcedSecurityCode
        {
            get
            {
                object value = this.GetProperty(ISFORCEDSECURITYCODE);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISFORCEDSECURITYCODE, value);
            }
        }

        /// <summary>
        /// Security code of current user
        /// </summary>
        public string SecurityCode
        {
            get
            {
                object value = this.GetProperty(SECURITYCODE);
                return (null == value) ? null : (string)value;
            }

            set
            {
                this.SetProperty(SECURITYCODE, value);
            }
        }

        public SCResult SecurityCodeResult
        {
            get
            {
                SCResult scResult;
                object value = this.GetProperty(SECURITYCODERESULT);
                if (null == value)
                {
                    bool isForceSC = IsForcedSecurityCode;
                    bool hasSC = !string.IsNullOrEmpty(SecurityCode);
                    //// forced change SC
                    if (isForceSC)
                    {
                        if (hasSC)
                        {
                            scResult = SCResult.FORCED_CHANGE_SC;
                        }
                        else
                        {
                            scResult = SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION;
                        }
                    }
                    else
                    {
                        if (hasSC)
                        {
                            scResult = SCResult.PROVIDED_SC;
                        }
                        else
                        {
                            scResult = SCResult.NONE;

                            /// if current datetime > deadline, force user inputs his SC
                            DateTime deadline;
                            if (DateTime.TryParseExact(ConfigurationManager.AppSettings["DeadlineEnableSecurity"], "MM/dd/yyyy", null, DateTimeStyles.None, out deadline))
                            {
                                if (DateTime.Now > deadline)
                                {
                                    scResult = SCResult.PROVIDED_SC;
                                }
                            }
                        }
                    }

                    // assign value
                    this.SetProperty(SECURITYCODERESULT, scResult);
                }
                else
                {
                    scResult = (SCResult)value;
                }

                return scResult;
            }

            set
            {
                this.SetProperty(SECURITYCODERESULT, value);
            }
        }

        public bool ShowSecurityCodePopup
        {
            get
            {
                object value = this.GetProperty(SHOWSECURITYCODEPOPUP);
                return (null == value) ? true : (bool)value;
            }

            set
            {
                this.SetProperty(SHOWSECURITYCODEPOPUP, value);
            }
        }

        public string SecurityCodeToken
        {
            get
            {
                object value = this.GetProperty(SECURITYCODETOKEN);
                return (null == value) ? string.Empty : (string)value;
            }

            set
            {
                this.SetProperty(SECURITYCODETOKEN, value);
            }
        }

        public bool IsEnabledSecurityCodeInBetLists
        {
            get
            {
                var value = GetProperty(ISENABLEDSECURITYCODEINBETLISTS);
                return null == value ? false : (bool)value;
            }

            set
            {
                SetProperty(ISENABLEDSECURITYCODEINBETLISTS, value);
            }
        }

        public bool IsEnabledSecurityCodeInStatement
        {
            get
            {
                var value = GetProperty(ISENABLEDSECURITYCODEINSTATEMENT);
                return null == value ? false : (bool)value;
            }

            set
            {
                SetProperty(ISENABLEDSECURITYCODEINSTATEMENT, value);
            }
        }

        #endregion Logged-in informations            

        #region Double Comm Losing

        public bool IsDoubleCommLosing
        {
            get
            {
                object value = this.GetProperty(ISDOUBLECOMMLOSING);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISDOUBLECOMMLOSING, value);
            }
        }

        #endregion Double Comm Losing

        #region Virtual Sports

        public bool IsHideVirtualSports
        {
            get
            {
                object value = this.GetProperty(ISHIDEVIRTUALSPORTS);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISHIDEVIRTUALSPORTS, value);
            }
        }

        public bool IsDisabledVirtualSports
        {
            get
            {
                object value = this.GetProperty(ISDISABLEDVIRTUALSPORTS);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISDISABLEDVIRTUALSPORTS, value);
            }
        }

        #endregion Virtual Sports

        #region Keno

        public bool IsHideKeno
        {
            get
            {
                object value = this.GetProperty(ISHIDEKENO);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISHIDEKENO, value);
            }
        }

        #endregion Keno

        #region Sportsbook2

        public bool IsHideSportsbook2
        {
            get
            {
                object value = this.GetProperty(ISHIDESPORTSBOOK2);
                return (null == value) ? true : (bool)value;
            }

            set
            {
                this.SetProperty(ISHIDESPORTSBOOK2, value);
            }
        }

        public bool DisableSportsbook2
        {
            get
            {
                object value = this.GetProperty(DISABLESPORTSBOOK2);
                return (null == value) ? true : (bool)value;
            }

            set
            {
                this.SetProperty(DISABLESPORTSBOOK2, value);
            }
        }

        #endregion Sportsbook2

        /// <summary>
        /// Disabled Number Game status of logged-in user.
        /// </summary>
        public bool IsNumberGameDisabled
        {
            get
            {
                object value = this.GetProperty(ISNUMBERGAMEDISABLED);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISNUMBERGAMEDISABLED, value);
            }
        }

        #region PostionTaking

        public bool IsDisabledAutoMPT
        {
            get
            {
                object value = this.GetProperty(ISDISABLEDAUTOMPT);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISDISABLEDAUTOMPT, value);
            }
        }

        #endregion PostionTaking

        #region UplineInformations

        /// <summary>
        /// Super Id. Return 0 if it is not available.
        /// </summary>
        public int SRecommendId
        {
            get
            {
                object value = this.GetProperty(SRECOMMENDID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(SRECOMMENDID, value);
            }
        }

        /// <summary>
        /// Master Id. Return 0 if it is not available.
        /// </summary>
        public int MRecommendId
        {
            get
            {
                object value = this.GetProperty(MRECOMMENDID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(MRECOMMENDID, value);
            }
        }

        /// <summary>
        /// Agent Id. Return 0 if it is not available.
        /// </summary>
        public int ARecommendId
        {
            get
            {
                object value = this.GetProperty(ARECOMMENDID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                this.SetProperty(ARECOMMENDID, value);
            }
        }

        #endregion UplineInformations

        public string NetSessionId
        {
            get { return this.asNetSessionId; }
        }

        public bool IsLimitedHostPermisionOnSubAccountFunction
        {
            get
            {
                object value = this.GetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION, value);
            }
        }

        #region Show/Hide Menu

        public bool IsShowBetChoiceLimit
        {
            get
            {
                object value = this.GetProperty(ISSHOWBETCHOICELIMIT);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISSHOWBETCHOICELIMIT, value);
            }
        }

        #endregion

        public bool IsDisabledESports
        {
            get
            {
                object value = this.GetProperty(ISDISABLEDESPORTS);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISDISABLEDESPORTS, value);
            }
        }

        public bool IsMyanmarOdds
        {
            get
            {
                object value = this.GetProperty(ISMYANMARODDS);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISMYANMARODDS, value);
            }
        }

        public bool IsDisabledSuperLive
        {
            get
            {
                object value = this.GetProperty(DISABLESUPERLIVE);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(DISABLESUPERLIVE, value);
            }
        }

        public bool GetHideProduct(int productId)
        {
            object value = GetProperty(HIDEPRODUCT + "_" + productId);

            return (null == value) ? true : (bool)value;
        }

        public void SetHideProduct(int productId, bool value)
        {
            SetProperty(HIDEPRODUCT + "_" + productId, value);
        }

        public bool GetSyncedProduct(int productId)
        {
            object value = GetProperty(SYNCEDPRODUCT + "_" + productId);

            return (null == value) ? false : (bool)value;
        }

        public void SetSyncedProduct(int productId, bool value)
        {
            SetProperty(SYNCEDPRODUCT + "_" + productId, value);
        }

        public bool GetDisabledProduct(int productId)
        {
            object value = GetProperty(DISABLEDPRODUCT + "_" + productId);

            return (null == value) ? true : (bool)value;
        }

        public void SetDisabledProduct(int productId, bool value)
        {
            SetProperty(DISABLEDPRODUCT + "_" + productId, value);
        }

        public string UserLoginIp
        {
            get
            {
                object value = GetProperty(USERLOGINIP);
                return value as string;
            }

            set
            {
                SetProperty(USERLOGINIP, value);
            }
        }

        #region TotalBalanceTransfer

        public bool IsAllowedTotalBalanceTransfer
        {
            get
            {
                object value = this.GetProperty(ISALLOWEDTOTALBALANCETRANSFER);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                this.SetProperty(ISALLOWEDTOTALBALANCETRANSFER, value);
            }
        }

        #endregion TotalBalanceTransfer

        public bool IsInternalWebVPN
        {
            get
            {
                object property = this.GetProperty("ISINTERNALWEBVPN");
                return property != null && (bool)property;
            }
            set
            {
                this.SetProperty("ISINTERNALWEBVPN", value);
            }
        }

        public bool IsShowSkipForceChangePassword
        {
            get
            {
                return GetProperty<bool>(ISSHOWSKIPFORCECHANGPASSWORD);
            }

            set
            {
                SetProperty(ISSHOWSKIPFORCECHANGPASSWORD, value);
            }
        }

        public bool IsIndirectLoggedIn
        {
            get => IndirectCustId > 0 || IndirectSubAccId > 0;
        }

        public int IndirectCustId
        {
            get => GetProperty<int>(IndirectCustIdSessionKey);
            set => SetProperty(IndirectCustIdSessionKey, value);
        }

        public int IndirectSubAccId
        {
            get => GetProperty<int>(IndirectSubAccIdSessionKey);
            set => SetProperty(IndirectSubAccIdSessionKey, value);
        }

        public string IndirectUsername
        {
            get => GetProperty<string>(IndirectUsernameSessionKey);
            set => SetProperty(IndirectUsernameSessionKey, value);
        }

        public bool IsOfferProtectedMode
        {
            get => GetProperty<bool>(ISOFFERPROTECTEDMODESESSIONKEY);
            set => SetProperty(ISOFFERPROTECTEDMODESESSIONKEY, value);
        }

        public bool IsProtectedMode
        {
            get => GetProperty<bool>(ISPROTECTEDMODESESSIONKEY);
            set => SetProperty(ISPROTECTEDMODESESSIONKEY, value);
        }

        public string DarkModeName
        {
            get => GetProperty<string>(DARKMODENAMESESSIONKEY) ?? string.Empty;
            set => SetProperty(DARKMODENAMESESSIONKEY, value);
        }

        public string AssetAppPath
        {
            get => GetProperty<string>(ASSETAPPPATHSESSIONKEY) ?? string.Empty;
            set => SetProperty(ASSETAPPPATHSESSIONKEY, value);
        }

        public bool AutoFollowBetIsDummyAgent
        {
            get => GetProperty<bool>(AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY);
            set => SetProperty(AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY, value);
        }

        public T GetProperty<T>(string propertyName)
        {
            var value = GetProperty(propertyName);

            return value == null ? default(T) : (T)value;
        }

        public void SetProperty(string propertyName, object value)
        {
            HttpContext.Current.Session[this.asNetSessionId + HYPHEN + propertyName] = value;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Type type = GetType();
            FieldInfo[] f = type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);

            int c = f.Length;
            for (int i = 0; i < c; i++)
            {
                info.AddValue(f[i].Name, f[i].GetValue(this));
            }
        }

        #endregion ISerializable Members

        #region IDisposable Members

        public void Dispose()
        {
            Type type = GetType();
            PropertyInfo[] p = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

            int c = p.Length;
            for (int i = 0; i < c; i++)
            {
                this.SetProperty(p[i].Name.ToUpper(), null);
            }
        }

        #endregion IDisposable Members

        #region Controller

        private object GetProperty(string name)
        {
            return HttpContext.Current.Session[this.asNetSessionId + HYPHEN + name];
        }

        #endregion Controller
    }
}