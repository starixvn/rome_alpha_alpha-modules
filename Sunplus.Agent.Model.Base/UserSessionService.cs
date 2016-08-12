namespace Sunplus.Agent.Model.BL
{
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;
    using Fanex.Common;
    using Model.Base.Configurations;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Definitions;
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
        public static IUserSession CreateUserSessionInstance(Hashtable values, string netSessionId)
        {
            UserSessionBase session = new UserSessionBase(netSessionId);

            SetUserSessionValues(session, values);

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
        /// bool disablebingo,
        /// bool syncbingo
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
        public static void SetUserSessionValues(IUserSession userSession, Hashtable values)
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
            }
            catch
            {
                session.IsPasswordExpired = false;
            }

            session.IsCasinoDisabled = (bool)values["iscasinodisabled"];
            session.IsCasinoSync = (bool)values["iscasinosync"];

            if (values.Contains("disablep2p"))
            {
                session.IsP2PDisabled = (bool)values["disablep2p"];
            }

            if (values.Contains("disablelivecasino"))
            {
                session.IsLiveCasinoDisabled = (bool)values["disablelivecasino"];
            }

            if (values.Contains("synclivecasino"))
            {
                session.IsLiveCasinoSync = (bool)values["synclivecasino"];
            }

            if (values.Contains("doublecommlosing"))
            {
                session.IsDoubleCommLosing = (bool)values["doublecommlosing"];
            }

            if (values.Contains("disablevirtualsports"))
            {
                session.IsDisabledVirtualSports = (bool)values["disablevirtualsports"];
            }

            if (values.Contains("hidevirtualsports"))
            {
                session.IsHideVirtualSports = (bool)values["hidevirtualsports"];
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

            if (values.Contains("disablenumber"))
            {
                session.IsNumberGameDisabled = (bool)values["disablenumber"];
            }

            if (values.Contains("disablebingo"))
            {
                session.IsBingoDisabled = (bool)values["disablebingo"];
            }

            if (values.Contains("syncbingo"))
            {
                session.IsBingoSync = (bool)values["syncbingo"];
            }

            session.OnUserId = (int)values["on_userid"];

            // Extra informations
            if (values.Contains("disable151_41"))
            {
                session.IsDisable151_31 = (bool)values["disable151_31"];
                session.IsDisable151_41 = (bool)values["disable151_41"];
                session.IsDisable151_51 = (bool)values["disable151_51"];
                session.IsDisable152_41 = (bool)values["disable152_41"];
                session.IsDisable152_51 = (bool)values["disable152_51"];
                session.IsDisable153_51 = (bool)values["disable153_51"];
            }

            // Will be removed when Racing was launched
            if (values.Contains("disablehorseracing"))
            {
                session.IsDisabledHR = (bool)values["disablehorseracing"];
            }

            session.IsHideKeno = !values.Contains("hidekeno") || (bool)values["hidekeno"];
            session.IsHideGoldDeluxe = !values.Contains("hidegolddeluxe") || (bool)values["hidegolddeluxe"];
            session.DisableGoldDeluxe = !values.Contains("disablegolddeluxe") || (bool)values["disablegolddeluxe"];

            session.IsHideColossusBets = !values.Contains("hidecolossusbet") || (bool)values["hidecolossusbet"];
            session.DisableColossusBets = !values.Contains("disablecolossusbet") || (bool)values["disablecolossusbet"];

            session.IsHideSportsbook2 = !values.Contains("hidemuaythai") || Converter.ToBool(values["hidemuaythai"]);
            session.DisableSportsbook2 = !values.Contains("disablemuaythai") || Converter.ToBool(values["disablemuaythai"]);

            SetProductsStatus(session, values);

            session.IsDisabledFI = (bool)values["disablefi"];
            session.IsDisabledAutoMPT = (bool)values["disableautompt"];
            session.SRecommendId = (int)values["srecommend"];
            session.MRecommendId = (int)values["mrecommend"];
            session.ARecommendId = (int)values["arecommend"];
            session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
            session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
            session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
            session.IsShowBetChoiceLimit = (bool)values["isShowBetChoiceLimit"];

            if (AppSettings.IsSupportMyanmarOdds)
            {
                session.IsMyanmarOdds = Converter.ToBool(values["ismyanmarodds"]);
            }

            if (AppSettings.IsEnabledProduct(ProductIdConstants.Cricket))
            {
                session.IsIMBCCustomer = Converter.ToBool(values["isimbc"]);
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

            if (values.Contains("disableesports"))
            {
                session.IsDisabledESports = (bool)values["disableesports"];
            }

            session.IsDisabledSuperLive = !values.Contains("disablesuperlive") || (bool)values["disablesuperlive"];
        }

        private static void SetProductsStatus(UserSessionBase session, Hashtable values)
        {
            string productName, storedProcedureName;
            var products = ProductManager.NewFlowProducts;

            foreach (var product in products)
            {
                if (product.Active)
                {
                    productName = product.Name.ToLower();

                    storedProcedureName = "hide" + productName;
                    if (values.ContainsKey(storedProcedureName))
                    {
                        session.SetHideProduct(product.Id, Converter.ToBool(values[storedProcedureName]));
                    }

                    storedProcedureName = "disable" + productName;
                    if (values.ContainsKey(storedProcedureName))
                    {
                        session.SetDisabledProduct(product.Id, Converter.ToBool(values[storedProcedureName]));
                    }

                    storedProcedureName = "synced" + productName;
                    if (values.ContainsKey(storedProcedureName))
                    {
                        session.SetSyncedProduct(product.Id, Converter.ToBool(values[storedProcedureName]));
                    }
                }
                else
                {
                    session.SetHideProduct(product.Id, true);
                }
            }
        }
    }
}