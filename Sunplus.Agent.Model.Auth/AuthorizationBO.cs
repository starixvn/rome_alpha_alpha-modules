namespace Sunplus.Agent.Model.BL.Authorization
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Model.BL.MemberInfo.SecurityCode;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.DL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [Obsolete("Should rename to Sunplus.Agent.Model.Auth")]
    public class AuthorizationBO : Base.BOBase
    {
        private AuthorizationDAO dao = new AuthorizationDAO();

        public AuthorizationBO(string netSessionId)
            : base(netSessionId)
        {
        }

        /// <summary>
        /// Sign out due to session timeout.
        /// </summary>
        public static void SignOut2(int onUserId, string userName, string nickName, byte roleId, int serverId)
        {
            AuthorizationDAO dao = new AuthorizationDAO();
            dao.Acc_CheckOut(onUserId, userName, nickName, roleId, serverId);
        }

        public bool IsSynchronized(int custId)
        {
            Hashtable outputs = new Hashtable();
            this.GetCustomerStatus(custId, CurrentSession.AdminId, ref outputs);

            return (bool)outputs["synccasino"];
        }

        /// <summary>
        /// Customer sign-in system and get necessary informations.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public bool SignIn(string userName, string password, Hashtable requestInfo, out int errCode, out string errMsg, ref IUserSession userSession)
        {
            Hashtable outputs = new Hashtable();
            bool isSubAcc;

            // Do sign-in
            bool result = this.dao.SignIn(
                userName,
                password,
                userSession.LangKey,
                userSession.NetSessionId,
                requestInfo,
                out errCode,
                out errMsg,
                ref outputs,
                out isSubAcc);

            int custId = (int)outputs["custid"]; // Equal zero if log-in failed

            if (!result)
            {
                try
                {
                    DateTime lastChangedPwd = (DateTime)outputs["lastchangedpwddate"];
                    userSession.LastChangedPassword = lastChangedPwd;
                }
                catch
                {
                    userSession.LastChangedPassword = DateTime.MinValue;
                }

                try
                {
                    DateTime pwdExpiry = (DateTime)outputs["pwdexpiry"];
                    userSession.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex, TraceEventType.Information);
                    userSession.IsPasswordExpired = false;
                }

                if (errCode == 108)
                {
                    try
                    {
                        // Close status need to synchronize to Casino if login failed 5 times
                        CasinoBO casino = new CasinoBO(NetSessionId);
                        casino.Customer.UpdateCustClosedWrongPwd(custId, true, AgentIPFormat.GetClientIP());
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex, TraceEventType.Information);
                    }
                }

                return false;
            }

            int roleId = (int)outputs["roleid"];

            // Site name
            outputs.Add("sitename", Sunplus.Agent.Model.BL.Definitions.AppSettings.SiteNameFromRoleId(roleId));
            outputs.Add("subaccname", isSubAcc ? userName.ToUpperInvariant() : string.Empty);

            if (!isSubAcc)
            {
                if (!outputs.Contains("username"))
                {
                    outputs["username"] = userName;
                }

                outputs.Add("subaccid", 0);
                var pageAccess = new PageAccess();
                outputs["subpageaccess"] = pageAccess;
            }

            int onUserId;
            int serverId = (int)outputs["serverid"];
            int subAccId = (int)outputs["subaccid"];
            this.dao.Acc_Checkin(custId, subAccId, userSession.NetSessionId, serverId, requestInfo["loginip"].ToString(), out onUserId);
            outputs.Add("on_userid", onUserId);

            this.GetCustomerStatus(custId, 0, ref outputs);

            outputs.Add("adminid", 0);
            bool isShowConsolidated = ConfigurationManager.AppSettings["EnableConsolidatedReport"] == "1" && this.IsShownConsilatedReport(custId);
            outputs.Add("isshowconsolidated", isShowConsolidated);

            bool isShowBetChoiceLimit = IsShownBetChoiceLimit(custId);
            outputs.Add("isShowBetChoiceLimit", isShowBetChoiceLimit);

            UserSessionService.SetUserSessionValues(CurrentSession, outputs);

            return result;
        }

        /// <summary>
        /// Customer after sign-in system and get necessary informations.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public bool AfterSignIn(int custId, string password, int subAccId, string nickname, bool isNickname, DateTime loginTime, Hashtable requestInfo, out int errCode, out string errMsg, ref IUserSession userSession)
        {
            Hashtable outputs = new Hashtable();

            bool isSubAcc = subAccId != 0;

            // Do sign-in
            outputs = this.dao.Age_LoginClaimDownline(
                custId,
                password,
                subAccId,
                nickname,
                isNickname,
                loginTime,
                userSession.LangKey,
                requestInfo,
                out errCode,
                out errMsg);

            if (errCode != 0)
            {
                userSession = null;
                return false;
            }

            outputs.Add("custid", custId);
            int roleId = (int)outputs["roleid"];
            string userName = outputs["username"] as string;

            // Site name
            outputs.Add("sitename", Sunplus.Agent.Model.BL.Definitions.AppSettings.SiteNameFromRoleId(roleId));

            outputs.Add("subaccid", subAccId);
            outputs.Add("subaccname", isSubAcc ? outputs["subusername"].ToString().ToUpperInvariant() : string.Empty);
            if (isSubAcc)
            {
                outputs.Remove("subusername");
            }

            int onUserId;
            int serverId = (int)outputs["serverid"];
            this.dao.Acc_Checkin(custId, subAccId, userSession.NetSessionId, serverId, requestInfo["loginip"].ToString(), out onUserId);
            outputs.Add("on_userid", onUserId);

            this.GetCustomerStatus(custId, 0, ref outputs);

            outputs.Add("adminid", 0);
            bool isShowConsolidated = ConfigurationManager.AppSettings["EnableConsolidatedReport"] == "1" && this.IsShownConsilatedReport(custId);
            outputs.Add("isshowconsolidated", isShowConsolidated);

            bool isShowBetChoiceLimit = IsShownBetChoiceLimit(custId);
            outputs.Add("isShowBetChoiceLimit", isShowBetChoiceLimit);

            UserSessionService.SetUserSessionValues(CurrentSession, outputs);

            return errCode == 0;
        }

        /// <summary>
        /// Override: Customer after sign-in system and get necessary informations.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public bool AfterSignIn(int custId, string password, int subAccId, DateTime loginTime, Hashtable requestInfo, out int errCode, out string errMsg, ref IUserSession userSession)
        {
            return this.AfterSignIn(custId, password, subAccId, null, false, loginTime, requestInfo, out errCode, out errMsg, ref userSession);
        }

        /// <summary>
        /// Administrator sign-in system as a customer and get necessary informations.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adminId"></param>
        /// <param name="loginTime"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public bool InternalSignIn(string userName, int adminId, DateTime loginTime, Hashtable requestInfo, out int errCode, out string errMsg, ref IUserSession userSession, string systemLogin = "NAP")
        {
            Hashtable outputs = this.dao.Age_LoginDownline_Admin(userName, adminId, loginTime, CurrentSession.LangKey, requestInfo, out errCode, out errMsg);

            if (errCode != 0)
            {
                userSession = null;
                return false;
            }

            int custId = (int)outputs["custid"]; // Equal zero if log-in failed
            int roleId = (int)outputs["roleid"];
            if (!outputs.Contains("username"))
            {
                outputs["username"] = userName;
            }

            // Site name
            outputs.Add("sitename", Sunplus.Agent.Model.BL.Definitions.AppSettings.SiteNameFromRoleId(roleId));

            outputs.Add("subaccid", 0); // Sub-account always be zero in internal login case
            outputs.Add("subaccname", string.Empty);

            int onUserId;
            int serverId = (int)outputs["serverid"];
            int subAccId = (int)outputs["subaccid"];
            this.dao.Acc_Checkin(custId, -adminId, userSession.NetSessionId, serverId, requestInfo["loginip"].ToString(), out onUserId);

            // on_userid already existed in internal login case
            outputs["on_userid"] = onUserId;

            this.GetCustomerStatus(custId, adminId, ref outputs, systemLogin);

            // Cast un-unique fields with normal SignIn
            outputs.Add("iscasinodisabled", outputs["casinodisable"]);
            outputs.Remove("casinodisable");
            outputs.Add("iscasinosync", outputs["casinosync"]);
            outputs.Remove("casinosync");
            outputs.Add("adminid", adminId);

            bool isShowConsolidated = ConfigurationManager.AppSettings["EnableConsolidatedReport"] == "1" && this.IsShownConsilatedReport(custId);
            outputs.Add("isshowconsolidated", isShowConsolidated);

            bool isShowBetChoiceLimit = IsShownBetChoiceLimit(custId);
            outputs.Add("isShowBetChoiceLimit", isShowBetChoiceLimit);

            UserSessionService.SetUserSessionValues(CurrentSession, outputs);

            return errCode == 0;
        }

        /// <summary>
        /// Check-out On_User database. Remove user session.
        /// </summary>
        public void SignOut()
        {
            this.dao.Acc_CheckOut(CurrentSession.OnUserId, CurrentSession.SubAccId > 0 ? CurrentSession.SubUserName : CurrentSession.UserName, CurrentSession.IsNickname ? CurrentSession.NickName : string.Empty, CurrentSession.RoleId, CurrentSession.ServerId);

            UserSessionService.RemoveUserSession(CurrentSession);
        }

        /// <summary>
        /// True if re-checkin successful. False for kick-out.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ReCheckin(out string errMsg)
        {
            return this.dao.Acc_ReCheckin(CurrentSession.CustId, CurrentSession.OnUserId, out errMsg);
        }

        #region Checking Permission

        /// <summary>
        /// Check permission of a sub-account.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckPageAccess(string type, out int errCode, out string errMsg)
        {
            bool canAccess = this.dao.Age_CustSubCheckPageAccess(CurrentSession.SubAccId, type, CurrentSession.LangKey, CurrentSession.UserId, out errCode, out errMsg);
            if (errCode == 105)
            {
                this.SignOut();
            }

            return canAccess;
        }

        /// <summary>
        /// Get some extra informations of a customer.
        /// [bool disablehorseracing, bool disablefi, bool disableautompt, int srecommend, int mrecommend, int recommend, int isadmincanviewvnip,suspend,closed,autosuspend,synccasino,disablecasino,disableoutright,IsInternal].
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="adminId"></param>
        /// <param name="outputs"></param>
        /// <param name="systemLogin"></param>
        /// <returns></returns>
        public void GetCustomerStatus(int custId, int adminId, ref Hashtable outputs, string systemLogin = "NAP")
        {
            DataTable dt = this.dao.Acc_CustomerSelStatus(custId, adminId, systemLogin);

            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                foreach (DataColumn column in dt.Columns)
                {
                    outputs.Add(column.ColumnName.ToLower(), row[column]);
                }
            }

            bool isAllowedTransferTotalBalance = this.dao.Age_Transfer_CheckTotalBalanceTransfer(custId, Sunplus.Agent.Model.BL.Definitions.AppSettings.AllowedListSuperAccounts);
            outputs.Add("isallowedtotalbalancetransfer", isAllowedTransferTotalBalance);
        }

        /// <summary>
        /// Checking Show/Hide Consolidated Report
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public bool IsShownConsilatedReport(int custId)
        {
            return this.dao.Age_Rpt_ConsolidatedGroup_isShownMenu(custId);
        }

        public bool IsShownBetChoiceLimit(int custId)
        {
            return dao.Age_CustSetting_BetChoiceLimitCheck(custId);
        }

        /// <summary>
        /// Checking enable status of a customer.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool IsCustomerEnable(int custId, out string errMsg)
        {
            return this.dao.Acc_CustomerSelCheckEnable(custId, out errMsg);
        }

        /// <summary>
        /// Check request is belong to downline information.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="transId"></param>
        /// <param name="isDataWareHouse"></param>
        public bool CheckBelonging(int custId, long transId, bool isDataWareHouse)
        {
            return isDataWareHouse
                ? this.dao.Age_CustomerSelCheckBelonging_OLD(custId, CurrentSession.CustId, transId)
                : this.dao.Age_CustomerSelCheckBelonging(custId, CurrentSession.CustId, CurrentSession.SubAccId, transId, CurrentSession.IsInternal);
        }

        public bool CheckBelonging(string custIds)
        {
            return this.dao.Age_CustomerSelCheckBelonging_Multi(custIds, CurrentSession.CustId, CurrentSession.SubAccId, CurrentSession.IsInternal);
        }

        #endregion Checking Permission

        /// <summary>
        /// Authorize a http request by checking it's param: custid, cid, recommend, mrecommend, transid, refno, isWHCheck.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="transId"></param>
        /// <param name="isDataWarehouse"></param>
        /// <param name="urlKey"></param>
        /// <returns></returns>
        public bool AuthorizeUrl(int custId, long transId, bool isDataWarehouse, string url)
        {
            // Check Role and PageAccess
            PageAccess pageAccess = CurrentSession.SubPageAccess;
            byte currentRoleId = CurrentSession.RoleId;

            string pageAccessString = (pageAccess == null) ? string.Empty : (string)pageAccess;
            bool isSubAcc = CurrentSession.IsSubAcc;

            PageFunction pageFunction = PageFunctionCollection.GetPageFunctionByUrl(url);
            if (pageFunction == null)
            {
                return true; // In case url is not described in PageAccess.xml
            }

            RolePermissionPair[] roleRules = pageFunction.RoleRules;
            int length = roleRules.Length;

            // Match at least one role rule
            for (int i = 0; i < length; i++)
            {
                RolePermissionPair rule = roleRules[i];
                if (rule.Role <= currentRoleId)
                {
                    if (!isSubAcc || pageAccessString.Contains(rule.Permission))
                    {
                        //// Update page statistic
                        if (!CurrentSession.IsInternal)
                        {
                            var statistic = new Statistic.StatisticBO(NetSessionId);

                            //// statistic.UpdatePageVisit_Old(url);

                            statistic.UpdateFunctionUsage(pageFunction.Url, pageFunction.Name, pageFunction.SubName, CurrentSession.Site);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        #region ViewLogDependencies

        public bool IsUsernameBelong(string userName, int uplineId, out int custId, out bool isSubAcc)
        {
            // lewin DO NOT check belonging
            custId = 0;
            isSubAcc = false;
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            var dao = new AuthorizationDAO();
            custId = dao.Age_CustomerSelCheckBelonging_ByUsername(userName, uplineId, out isSubAcc);
            return custId > 0;
        }

        /// <summary>
        /// Check user whether has permission on a function or not
        /// Author: Lewin
        /// Task: viewlog function
        /// </summary>
        /// <param name="userId">int-userid want to check his permission</param>
        /// <param name="funcId">int-functionid</param>
        /// <returns>bool</returns>
        public bool CheckUserPermission(int userId, int funcId)
        {
            if (userId <= 0 || funcId <= 0)
            {
                return false;
            }

            var hasPermission = this.dao.Age_CustomerSelCheckPermission(userId, funcId);

            return hasPermission;
        }

        #endregion ViewLogDependencies

        #region Search&Copy

        /// <summary>
        /// Check if userName is belong to uplineId.
        /// Author: Jeff
        /// Task: Search&Copy
        /// </summary>
        /// <param name="userId">int-userid want to check his permission</param>
        /// <param name="funcId">int-functionid</param>
        /// <returns>bool</returns>
        public bool IsUsernameBelong(string userName, int uplineId, out int custId, out bool isSubAcc, out int roleId)
        {
            custId = 0;
            isSubAcc = false;
            roleId = 0;
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            var dao = new AuthorizationDAO();
            custId = dao.Age_CustomerSelCheckBelonging_ByUsername(userName, uplineId, out isSubAcc, out roleId);
            return custId > 0;
        }

        #endregion Search&Copy

        #region Security code

        /// <summary>
        /// Change security code of current customer
        /// </summary>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int ChangeSecurityCode(string safeCode, string ip, out string errMsg)
        {
            SecurityCodeBO bo = new SecurityCodeBO(NetSessionId);
            return bo.ChangeSecurityCode(safeCode, ip, out errMsg);
        }

        /// <summary>
        /// Check security code in DB
        /// </summary>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int CheckSecurityCode(string safeCode, string ip, out string errMsg, bool isSessionSaving)
        {
            SecurityCodeBO bo = new SecurityCodeBO(NetSessionId);
            return bo.CheckSecurityCode(safeCode, ip, out errMsg, isSessionSaving);
        }

        #endregion Security code
    }
}