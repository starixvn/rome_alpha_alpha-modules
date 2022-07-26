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
        /// bool disablenumber,
        /// int on_userid,      
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

            try
            {
                session.CustId = Convert.ToInt32(values["custid"]);
                session.UserName = (values["username"] as string)?.ToUpperInvariant().Trim();
                session.SubAccId = Convert.ToInt32(values["subaccid"]);

                session.SubUserName = values["subaccname"] as string;
                session.SubPageAccess = (PageAccess)(values["subpageaccess"] as string) ?? new PageAccess();
                if (!session.IsSubAcc)
                {
                    session.SubPageAccess.SetFullPermission();
                }

                session.SiteName = values["sitename"] as string;
                session.Site = values["site"] as string;

                session.Currency = values["currency"] as string;
                session.CurrencyId = Convert.ToInt32(values["currencyid"]);
                session.RoleId = Convert.ToByte(values["roleid"]);
                session.ServerId = Convert.ToInt32(values["serverid"]);

                session.AdminId = Convert.ToInt32(values["adminid"]);
                session.AdminName = values["accusername"] as string;

                session.IsLogged = session.IsInternal || !Convert.ToBoolean(values["isfirstlogin"]);
            }
            catch
            {
            }

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

                var defaultDateSkipForceChangePass = new DateTime(1900, 1, 1);
                session.IsShowSkipForceChangePassword = session.IsLogged && DateTime.Compare(pwdExpiry, defaultDateSkipForceChangePass) > 0;
            }
            catch
            {
                session.IsPasswordExpired = false;
                session.IsShowSkipForceChangePassword = false;
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
                session.IsNicknameLoginOnly = !string.IsNullOrEmpty(values["nicknameloginonly"] + string.Empty) && (bool)values["nicknameloginonly"];
            }

            if (values.Contains("isnickname"))
            {
                session.IsNickname = !string.IsNullOrEmpty(values["isnickname"] + string.Empty) && (bool)values["isnickname"];
            }

            session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

            if (values.Contains("on_userid") && values["on_userid"] != null && values["on_userid"] != DBNull.Value)
            {
                session.OnUserId = int.Parse(values["on_userid"].ToString());
            }

            SetProductStatus(session, values);
            SetProductStatusToDictionary(session, values);

            setUserStatusAction(session, values);

            session.IsDisabledAutoMPT = (bool)values["disableautompt"];
            session.SRecommendId = Convert.ToInt32(values["srecommend"]);
            session.MRecommendId = Convert.ToInt32(values["mrecommend"]);
            session.ARecommendId = Convert.ToInt32(values["arecommend"]);
            session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
            session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
            session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
            session.IsShowBetChoiceLimit = (bool)values["isShowBetChoiceLimit"];

            if (AppSettings.IsSupportMyanmarOdds)
            {
                session.IsMyanmarOdds = Converter.ToBool(values["ismyanmarodds"]);
            }

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

            session.IsNumberGameDisabled = GetProductStatus("disablenumber", values, true);

            session.IsHideKeno = GetProductStatus("hidekeno", values, true);       

            session.IsHideSportsbook2 = GetProductStatus("hidemuaythai", values, true);
            session.DisableSportsbook2 = GetProductStatus("disablemuaythai", values, true);

            session.IsDisabledESports = GetProductStatus("disableesports", values, true);

            session.IsDisabledSuperLive = GetProductStatus("disablesuperlive", values, true);
        }

        private static void SetProductStatusToDictionary(UserSessionBase session, Hashtable values)
        {
            ProductManager.SetProductsStatusToSession(values);           
            session.SetDisabledProduct(ProductIdConstants.NumberGame, GetProductStatus("disablenumber", values, true));
            session.SetHideProduct(ProductIdConstants.Keno, GetProductStatus("hidekeno", values, true));
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