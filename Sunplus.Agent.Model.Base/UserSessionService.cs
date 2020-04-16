namespace Sunplus.Agent.Model.BL
{
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;
    using Fanex.Common;
    using Fanex.Products;
    using Model.Base;
    using Model.Definitions;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;

    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public sealed class UserSessionService
    {
        /// <summary>
        /// Get user session from current session id.
        /// </summary>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        public static IUserSession GetUserSessionInstance(string netSessionId)
        {
            return new UserSessionBase(netSessionId) as IUserSession;
        }

        /// <summary>
        /// Remove an user session.
        /// </summary>
        /// <param name="userSession"></param>
        public static void RemoveUserSession(IUserSession userSession)
        {
            UserSessionBase session = userSession as UserSessionBase;
            if (session != null)
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Create an empty user session.
        /// </summary>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        public static IUserSession CreateUserSessionInstance(string netSessionId)
        {
            return new UserSessionBase(netSessionId);
        }

        /// <summary>
        /// Create a user session with a session id and values.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        public static IUserSession CreateUserSessionInstance(Hashtable values, string netSessionId, Action<IUserSession, Hashtable> SetUserStatusAction)
        {
            UserSessionBase session = new UserSessionBase(netSessionId);

            SetUserSessionValues(session, values, SetUserStatusAction);

            return session;
        }

        /// <summary>
        /// Init user session from a hash table of values:
        /// int custid,
        /// string userName,
        /// string nickname,
        /// int subaccid,
        /// string subaccname,
        /// string subpageaccess,
        /// string sitename,
        /// string site,
        /// string currency,
        /// int currencyid,
        /// int roleid,
        /// int serverid,
        /// int adminid,
        /// string accusername,
        /// string loginip,
        /// bool isallowhrcurrency,
        /// bool iscasinodisabled,
        /// bool iscasinosync,
        /// bool disablenumber,
        /// int on_userid,
        /// bool disablehorseracing,
        /// bool disablefi,
        /// bool disableautompt,
        /// int srecommend,
        /// bool isadmincanviewvnip,
        /// bool isallowedtotalbalancetransfer,
        /// bool IsLogNickname
        /// bool isforcedchangesecuritycode
        /// bool securitycode
        /// </summary>
        /// <param name="userSession"></param>
        /// <param name="values"></param>
        public static void SetUserSessionValues(IUserSession userSession, Hashtable values, Action<IUserSession, Hashtable> setUserStatusAction)
        {
            if (null == userSession)
            {
                return;
            }

            UserSessionBase session = userSession as UserSessionBase;

            session.CustId = (int)values["custid"];
            session.UserName = (values["username"] as string).ToUpperInvariant().Trim();
            session.SubAccId = (int)values["subaccid"];

            session.SubUserName = values["subaccname"] as string;
            session.SubPageAccess = (PageAccess)(values["subpageaccess"] as string);
            if (!session.IsSubAcc)
            {
                session.SubPageAccess.SetFullPermission();
            }

            session.SiteName = values["sitename"] as string;
            session.Site = values["site"] as string;

            session.Currency = values["currency"] as string;
            session.CurrencyId = (int)values["currencyid"];
            session.RoleId = (byte)(int)values["roleid"];
            session.ServerId = (int)values["serverid"];

            session.AdminId = (int)values["adminid"];
            session.AdminName = values["accusername"] as string;

            session.IsLogged = session.IsInternal || !(bool)values["isfirstlogin"];

            try
            {
                // Security code
                session.IsForcedSecurityCode = (bool)values["isforcedchangesecuritycode"];
                session.SecurityCode = values["securitycode"] as string;
            }
            catch
            {
            }

            try
            {
                DateTime lastChangedPwd = (DateTime)values["lastchangedpwddate"];
                session.LastChangedPassword = lastChangedPwd;
            }
            catch
            {
                session.LastChangedPassword = DateTime.MinValue;
            }

            try
            {
                DateTime pwdExpiry = (DateTime)values["pwdexpiry"];
                session.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;
                session.IsSkipForceChangePassword = true;
            }
            catch
            {
                session.IsPasswordExpired = false;
                session.IsSkipForceChangePassword = false;
            }

            if (values.Contains("doublecommlosing"))
            {
                session.IsDoubleCommLosing = (bool)values["doublecommlosing"];
            }

            if (values.Contains("nickname"))
            {
                session.NickName = (values["nickname"] + string.Empty).ToUpperInvariant();
            }

            if (values.Contains("nicknameloginonly"))
            {
                session.IsNicknameLoginOnly = !string.IsNullOrEmpty(values["nicknameloginonly"].ToString()) && (bool)values["nicknameloginonly"];
            }

            if (values.Contains("isnickname"))
            {
                session.IsNickname = !string.IsNullOrEmpty(values["isnickname"].ToString()) && (bool)values["isnickname"];
            }

            session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

            session.OnUserId = (int)values["on_userid"];

            SetProductStatus(session, values);
            SetProductStatusToDictionary(session, values);

            setUserStatusAction(session, values);

            session.IsDisabledAutoMPT = (bool)values["disableautompt"];
            session.SRecommendId = (int)values["srecommend"];
            session.MRecommendId = (int)values["mrecommend"];
            session.ARecommendId = (int)values["arecommend"];
            session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
            session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
            session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
            session.IsShowBetChoiceLimit = (bool)values["isShowBetChoiceLimit"];

            if (!string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
                && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", session.AdminId), RegexOptions.IgnoreCase)
                && session.IsInternal)
            {
                session.IsLimitedHostPermisionOnSubAccountFunction = true;
            }
            else
            {
                session.IsLimitedHostPermisionOnSubAccountFunction = false;
            }

            session.IsAllDayBalTransfer = values.ContainsKey("isalldaybaltransfer") && (bool)values["isalldaybaltransfer"];
        }

        private static void SetProductStatus(UserSessionBase session, Hashtable values)
        {
            session.IsCasinoDisabled = (bool)values["iscasinodisabled"];
            session.IsCasinoSync = (bool)values["iscasinosync"];

            session.IsNumberGameDisabled = GetProductStatus("disablenumber", values, true);

            session.IsHideKeno = GetProductStatus("hidekeno", values, true);

            session.IsHideGoldDeluxe = GetProductStatus("hidegolddeluxe", values, true);
            session.DisableGoldDeluxe = GetProductStatus("disablegolddeluxe", values, true);

            session.IsHideSportsbook2 = GetProductStatus("hidemuaythai", values, true);
            session.DisableSportsbook2 = GetProductStatus("disablemuaythai", values, true);

            session.IsDisabledESports = GetProductStatus("disableesports", values, true);

            session.IsDisabledSuperLive = GetProductStatus("disablesuperlive", values, true);
        }

        private static void SetProductStatusToDictionary(UserSessionBase session, Hashtable values)
        {
            ProductManager.SetProductsStatusToSession(values);
            session.SetDisabledProduct(ProductIdConstants.RNGCasino, (bool)values["iscasinodisabled"]);
            session.SetSyncedProduct(ProductIdConstants.RNGCasino, (bool)values["iscasinosync"]);
            session.SetDisabledProduct(ProductIdConstants.NumberGame, GetProductStatus("disablenumber", values, true));
            session.SetHideProduct(ProductIdConstants.Keno, GetProductStatus("hidekeno", values, true));
            session.SetHideProduct(ProductIdConstants.GoldDeluxe, GetProductStatus("hidegolddeluxe", values, true));
            session.SetDisabledProduct(ProductIdConstants.GoldDeluxe, GetProductStatus("disablegolddeluxe", values, true));
            session.SetHideProduct(ProductIdConstants.Sportsbook2, GetProductStatus("hidemuaythai", values, true));
            session.SetDisabledProduct(ProductIdConstants.Sportsbook2, GetProductStatus("disablemuaythai", values, true));
            session.SetSyncedProduct(ProductIdConstants.LiveCasino, GetProductStatus("syncedlivecasino", values, true));

            ProductTransform.TransformProductStatus(session, values);
        }

        private static bool GetProductStatus(string productStatus, Hashtable values, bool defaultValue)
        {
            return values.Contains(productStatus) ? (bool)values[productStatus] : defaultValue;
        }
    }
}