namespace Sunplus.Agent.Model.BL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Fanex.Common;
    using Fanex.Products;
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
            
            if (values.ContainsKey("isimbc"))
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
        }

        private static Dictionary<int, string> ProductSpNames()
        {
            var result = new Dictionary<int, string>();
            result.Add(ProductIdConstants.CockFighting, "cockfighting");
            result.Add(ProductIdConstants.Cricket, "cricket");
            result.Add(ProductIdConstants.AllBet, "allbet");
            result.Add(ProductIdConstants.Voidbridge, "voidbridge");

            return result;
        }

        private static void SetProductStatus(UserSessionBase session, Hashtable values)
        {
            session.IsCasinoDisabled = (bool)values["iscasinodisabled"];
            session.IsCasinoSync = (bool)values["iscasinosync"];

            session.IsP2PDisabled = GetProductStatus("disablep2p", values, true);

            session.IsLiveCasinoDisabled = GetProductStatus("disablelivecasino", values, true);
            session.IsLiveCasinoSync = GetProductStatus("synclivecasino", values, false);

            session.IsDisabledVirtualSports = GetProductStatus("disablevirtualsports", values, true);
            session.IsHideVirtualSports = GetProductStatus("hidevirtualsports", values, true);

            session.IsNumberGameDisabled = GetProductStatus("disablenumber", values, true);

            session.IsBingoDisabled = GetProductStatus("disablebingo", values, true);
            session.IsBingoSync = GetProductStatus("syncbingo", values, false);

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
            session.IsDisabledHR = GetProductStatus("disablehorseracing", values, true);

            session.IsHideKeno = GetProductStatus("hidekeno", values, true);

            session.IsHideGoldDeluxe = GetProductStatus("hidegolddeluxe", values, true);
            session.DisableGoldDeluxe = GetProductStatus("disablegolddeluxe", values, true);

            session.IsHideColossusBets = GetProductStatus("hidecolossusbet", values, true);
            session.DisableColossusBets = GetProductStatus("disablecolossusbet", values, true);

            session.IsHideSportsbook2 = GetProductStatus("hidemuaythai", values, true);
            session.DisableSportsbook2 = GetProductStatus("disablemuaythai", values, true);

            session.IsDisabledESports = GetProductStatus("disableesports", values, true);

            session.IsDisabledSuperLive = GetProductStatus("disablesuperlive", values, true);

            session.IsSyncedAllBet = GetProductStatus("syncedallbet", values, false);
        }

        private static void SetProductStatusToDictionary(UserSessionBase session, Hashtable values)
        {

            var products = ProductSpNames();

            foreach (var product in products)
            {
                var storedProcedureName = "hide" + product.Value;
                session.SetHideProduct(product.Key, values.Contains(storedProcedureName) ? Converter.ToBool(values[storedProcedureName]) : true);

                storedProcedureName = "disable" + product.Value;
                session.SetDisabledProduct(product.Key, values.Contains(storedProcedureName) ? Converter.ToBool(values[storedProcedureName]) : true);

                storedProcedureName = "synced" + product.Value;
                session.SetSyncedProduct(product.Key, values.Contains(storedProcedureName) ? Converter.ToBool(values[storedProcedureName]) : false);
            }

            ProductManager.SetProductsStatusToSession(values);

            session.DisableProducts[ProductIdConstants.RNGCasino] = (bool)values["iscasinodisabled"];
            session.SyncProducts[ProductIdConstants.RNGCasino] = (bool)values["iscasinosync"];

            session.DisableProducts[ProductIdConstants.P2P] = GetProductStatus("disablep2p", values, true);

            session.DisableProducts[ProductIdConstants.LiveCasino] = GetProductStatus("disablelivecasino", values, true);
            session.SyncProducts[ProductIdConstants.LiveCasino] = GetProductStatus("synclivecasino", values, false);

            session.DisableProducts[ProductIdConstants.VirtualSports] = GetProductStatus("disablevirtualsports", values, true);
            session.HideProducts[ProductIdConstants.VirtualSports] = GetProductStatus("hidevirtualsports", values, true);

            session.DisableProducts[ProductIdConstants.NumberGame] = GetProductStatus("disablenumber", values, true);

            session.DisableProducts[ProductIdConstants.Bingo] = GetProductStatus("disablebingo", values, true);
            session.SyncProducts[ProductIdConstants.Bingo] = GetProductStatus("syncbingo", values, false);
            session.DisableProducts[ProductIdConstants.Racing] = GetProductStatus("disablehorseracing", values, true);

            session.HideProducts[ProductIdConstants.Keno] = GetProductStatus("hidekeno", values, true);

            session.HideProducts[ProductIdConstants.GoldDeluxe] = GetProductStatus("hidegolddeluxe", values, true);
            session.DisableProducts[ProductIdConstants.GoldDeluxe] = GetProductStatus("disablegolddeluxe", values, true);

            session.HideProducts[ProductIdConstants.ColossusBets] = GetProductStatus("hidecolossusbet", values, true);
            session.DisableProducts[ProductIdConstants.ColossusBets] = GetProductStatus("disablecolossusbet", values, true);

            session.HideProducts[ProductIdConstants.Sportsbook2] = GetProductStatus("hidemuaythai", values, true);
            session.DisableProducts[ProductIdConstants.Sportsbook2] = GetProductStatus("disablemuaythai", values, true);

            session.SyncProducts[ProductIdConstants.AllBet] = GetProductStatus("syncedallbet", values, false);
        }

        private static KeyValuePair<int, string>[] ProductSpNames()
        {
            return new KeyValuePair<int, string>[]
            {
                new KeyValuePair<int, string>(ProductIdConstants.CockFighting, "cockfighting"),
                new KeyValuePair<int, string>(ProductIdConstants.Cricket, "cricket"),
                new KeyValuePair<int, string>(ProductIdConstants.AllBet, "allbet"),
                new KeyValuePair<int, string>(ProductIdConstants.Voidbridge, "voidbridge")
            };
        }

        private static bool GetProductStatus(string productStatus, Hashtable values, bool defaultValue)
        {
            return values.Contains("productStatus") ? (bool)values["productStatus"] : defaultValue;
        }
    }
}