namespace Sunplus.Agent.Model.DL.Authorization
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Common;
    using Sunplus.Agent.Model.DL.Base;
    using Sunplus.Agent.Utilities;

    [Obsolete("Should rename to Sunplus.Agent.Model.Auth")]
    public class AuthorizationDAO : DAOBase
    {
        /// <summary>
        ///     SMA after login.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password"></param>
        /// <param name="subAccId"></param>
        /// <param name="nickname"></param>
        /// <param name="isNickname"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginClaimDownline(
            int custId,
            string password,
            int subAccId,
            string nickname,
            bool isNickname,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            {  
                { "custid", custId },
                { "userpwd", Encryption.CfsEnCode(password) },
                { "username", string.Empty },
                { "nickname", nickname },
                { "isnickname", isNickname },
                { "subaccid", subAccId },
                { "logintime", loginTime },
                { "language", language },
                { "roleid", 0 },
                { "on_userid", 0 } 
            };

            InsertRequestInfo(requestInfo, parameters);

            var outputs = ExecSpHashtable("Age_LoginClaimDownline", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        /// <summary>
        ///     Override SMA after login.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password">
        /// </param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginClaimDownline(
            int custId,
            string password,
            int subAccId,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            return Age_LoginClaimDownline(
                custId,
                password,
                subAccId,
                null,
                false,
                loginTime,
                language,
                requestInfo,
                out errCode,
                out errMsg);
        }

        /// <summary>
        ///     Admin login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adminId"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginDownline_Admin(
            string userName,
            int adminId,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            { 
                { "username", userName },
                { "accuserid", adminId },
                { "logintime", loginTime },
                { "language", language },
                { "roleid", 0 },
                { "custid", 0 },
                { "on_userid", 0 }
            };

            InsertRequestInfo(requestInfo, parameters);

            var outputs = ExecSpHashtable("Age_LoginDownline_Admin", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;

            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        /// <summary>
        ///     Overwrite Login Verify Downline
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="nickName"></param>
        /// <param name="isNickname"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="requestInfo"></param>
        public void Age_LoginVerifyDownline(
            string userName,
            string password,
            Hashtable requestInfo,
            string language,
            out int custId,
            out int subAccId,
            out DateTime loginTime,
            out string nickName,
            out bool isNickname,
            out int errCode,
            out string errMsg)
        {
            var lastChangedPwd = DateTime.MinValue;

            Age_LoginVerifyDownline(
                userName,
                password,
                requestInfo,
                language,
                out custId,
                out subAccId,
                out loginTime,
                out lastChangedPwd,
                out nickName,
                out isNickname,
                out errCode,
                out errMsg);
        }

        /// <summary>
        ///     Login Verify Downline
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="requestInfo"></param>
        /// <param name="language"></param>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="lastChangedPwd"></param>
        /// <param name="nickName"></param>
        /// <param name="isNickname"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void Age_LoginVerifyDownline(
            string userName,
            string password,
            Hashtable requestInfo,
            string language,
            out int custId,
            out int subAccId,
            out DateTime loginTime,
            out DateTime lastChangedPwd,
            out string nickName,
            out bool isNickname,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "username", userName },
                { "userpwd", Encryption.CfsEnCode(password) },
                { "custid", 0 },
                { "subaccid", 0 },
                { "serverid", 0 },
                { "logintime", DateTime.Now },
                { "language", language },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            InsertRequestInfo(requestInfo, parameters);

            var dr = ExecSp("Age_LoginVerifyDownline", parameters);
            isNickname = dr.Outputs.Contains("isnickname") && (bool)dr.Outputs["isnickname"];
            nickName = dr.Outputs.Contains("nickname") ? Convert.ToString(dr.Outputs["nickname"]) : string.Empty;
            errCode = (int)dr.Outputs["errcode"];
            errMsg = (string)dr.Outputs["errmsg"];
            custId = (int)dr.Outputs["custid"];
            subAccId = (int)dr.Outputs["subaccid"];
            loginTime = (DateTime)dr.Outputs["logintime"];

            try
            {
                lastChangedPwd = (DateTime)dr.Outputs["lastchangedpwddate"];
            }
            catch (Exception ex)
            {
                lastChangedPwd = DateTime.MinValue;
                Logs.Write(ex);
            }
        }

        /// <summary>
        ///     Override Login Verify Downline
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="language"></param>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="requestInfo"></param>
        public void Age_LoginVerifyDownline(
            string userName,
            string password,
            Hashtable requestInfo,
            string language,
            out int custId,
            out int subAccId,
            out DateTime loginTime,
            out int errCode,
            out string errMsg)
        {
            string nickName;
            var isNickname = false;

            Age_LoginVerifyDownline(
                userName,
                password,
                requestInfo,
                language,
                out custId,
                out subAccId,
                out loginTime,
                out nickName,
                out isNickname,
                out errCode,
                out errMsg);
        }

        /// <summary>
        ///     Customer/Sub-account login in smartphone site, waps site and after login site
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="netSessionId"></param>
        /// <param name="ip"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="outputValues"></param>
        /// <param name="isSubAcc"></param>
        /// <returns></returns>
        public bool SignIn(
            string userName,
            string password,
            string language,
            string netSessionId,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg,
            ref Hashtable outputValues,
            out bool isSubAcc)
        {
            var parameters = new Hashtable
            {
                { "isexist", 0 }, 
                { "subusername", userName }
            };

            var outputs = ExecSpHashtable("Age_LoginDownline_CheckSubUsername", parameters);

            if ((bool)outputs["isexisted"])
            {
                isSubAcc = true;
                outputValues = Age_LoginDownline_Sub(userName, password, language, requestInfo, out errCode, out errMsg);
            }
            else
            {
                isSubAcc = false;
                outputValues = Age_LoginDownline(userName, password, language, requestInfo, out errCode, out errMsg);
            }

            return errCode == 0;
        }

        /// <summary>
        ///     Check in a record on ON_USER database.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="netSessionId"></param>
        /// <param name="serverId"></param>
        /// <param name="ip"></param>
        /// <param name="onlineUserId"></param>
        public void Acc_Checkin(
            int custId,
            int subAccId,
            string netSessionId,
            int serverId,
            string ip,
            out int onlineUserId)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "netsessionid", netSessionId },
                { "serverid", serverId },
                { "on_userid", 0 },
                { "ip", ip },
                { "subaccid", subAccId }
            };

            //// Server ID Setting: agent: 90; master: 91; super: 92
            //// In order not to affect the online statistics report (admin site)

            var outputs = ExecSpHashtable("Acc_Checkin", parameters);
            onlineUserId = (int)outputs["on_userid"];
        }

        /// <summary>
        ///     True if re-checkin successful. False for kick-out.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="onlineUserId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Acc_ReCheckin(
            int custId,
            int onlineUserId,
            out string errMsg)
        {
            errMsg = string.Empty;

            if (onlineUserId <= 0)
            {
                errMsg = "System error(6090USL): You are forced to be kicked out.";
                return false;
            }

            var parameters = new Hashtable
            {
                { "on_userid", onlineUserId }, 
                { "iskickedout", 0 }
            };

            var outputs = ExecSpHashtable("Acc_ReCheckin", parameters);

            errMsg = "System error(6090USL): You are forced to be kicked out.";

            return !(bool)outputs["iskickedout"];
        }

        /// <summary>
        ///     Check-out On_User database.
        /// </summary>
        /// <param name="onlineUserId"></param>
        /// <param name="userName"></param>
        /// <param name="nickName"></param>
        /// <param name="roleId"></param>
        /// <param name="serverId"></param>
        public void Acc_CheckOut(
            int onlineUserId,
            string userName,
            string nickName,
            byte roleId,
            int serverId)
        {
            var parameters = new Hashtable
            {
                { "on_userid", onlineUserId },
                { "username", userName },
                { "roleid", roleId },
                { "serverid", serverId },
                { "nickname", nickName }
            };

            ExecSp("Acc_CheckOut", parameters);
        }

        #region Get Request Info

        private static void InsertRequestInfo(Hashtable requestInfo, Hashtable parameters)
        {
            foreach (DictionaryEntry d in requestInfo)
            {
                parameters.Add(d.Key, d.Value);
            }
        }

        #endregion

        /// <summary>
        ///     Sub-account login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private Hashtable Age_LoginDownline_Sub(
            string userName,
            string password,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "username", string.Empty },
                { "subusername", userName },
                { "userpwd", Encryption.CfsEnCode(password) },
                { "language", language },
                { "roleid", 0 },
                { "custid", 0 },
                { "on_userid", 0 }
            };

            InsertRequestInfo(requestInfo, parameters);

            var outputs = ExecSpHashtable("Age_LoginDownline_Sub", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            // Temporary remove. Waiting fixing store procedure.
            outputs.Remove("on_userid");

            return outputs;
        }

        /// <summary>
        ///     SMA login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="requestInfo"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private Hashtable Age_LoginDownline(
            string userName,
            string password,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "username", userName },
                { "userpwd", Encryption.CfsEnCode(password) },
                { "language", language },
                { "roleid", 0 },
                { "custid", 0 },
                { "on_userid", 0 }
            };

            InsertRequestInfo(requestInfo, parameters);

            var outputs = ExecSpHashtable("Age_LoginDownline", parameters);
            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        #region Checking Permission

        /// <summary>
        ///     Check permission of a sub-account.
        /// </summary>
        /// <param name="subAccId"></param>
        /// <param name="pageAccess"></param>
        /// <param name="language"></param>
        /// <param name="userId"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Age_CustSubCheckPageAccess(
            int subAccId,
            string pageAccess,
            string language,
            int userId,
            out int errCode,
            out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "subaccid", subAccId },
                { "sppageaccess", pageAccess },
                { "language", language },
                { "userid", userId },
                { "errmsg", string.Empty },
                { "errcode", 0 }
            };

            var outputs = ExecSpHashtable("Age_CustSubCheckPageAccess", parameters);

            errMsg = outputs["errmsg"] as string;
            errCode = (int)outputs["errcode"];

            return errCode == 0;
        }

        /// <summary>
        ///     Get some extra informations of a customer: [bool DisableHorseracing, bool DisableFI, bool DisableAutoMPT, int
        ///     SRecommend, MRecommend, ARecommend, int
        ///     IsAdminCanViewVNIP,Suspend,Closed,AutoSuspend,SyncCasino,DisableCasino,DisableOutright,IsInternal].
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="adminId"></param>
        /// <param name="systemLogin"></param>
        /// <returns>
        ///     [bool disablehorseracing, bool disablefi, bool disableautompt, int srecommend, mrecommend, arecommend, int
        ///     isadmincanviewvnip,suspend,closed,autosuspend,synccasino,disablecasino,disableoutright,isinternal]
        /// </returns>
        public DataTable Acc_CustomerSelStatus(int custId, int adminId, string systemLogin = "NAP")
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "adminid", adminId },
                { "system", systemLogin }
            };

            var result = ExecSpTable("Acc_CustomerSelStatus", parameters);

            return result;
        }

        /// <summary>
        ///     Check Show/Hide Consolidated Report
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public bool Age_Rpt_ConsolidatedGroup_isShownMenu(int custId)
        {
            var parameters = new Hashtable
            {
                { "custid", custId }, 
                { "isshownmenu", 0 }
            };
            var outputs = ExecSpHashtable("Age_Rpt_ConsolidatedGroup_isShownMenu", parameters);

            return (bool)outputs["isshownmenu"];
        }

        /// <summary>
        ///     Check a customer is allow transfer on Total Balance.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="superIds"></param>
        /// <returns></returns>
        public bool Age_Transfer_CheckTotalBalanceTransfer(int custId, string superIds)
        {
            var parameters = new Hashtable
            {
                { "custid", custId }, 
                { "superidstransfertotal", superIds }
            };

            var outputs = ExecSpHashtable("Age_Transfer_CheckTotalBalanceTransfer", parameters);

            return (bool)outputs["isallowed"];
        }

        /// <summary>
        ///     Checking a customer is enable or disabled.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Acc_CustomerSelCheckEnable(int custId, out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "custid", custId }, 
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var outputs = ExecSpHashtable("Acc_CustomerSelCheckEnable", parameters);

            errMsg = outputs["errmsg"] as string;
            var errCode = (int)outputs["errcode"];

            return errCode == 0;
        }

        /// <summary>
        ///     Check belonging in archive DB.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="currentCustId"></param>
        /// <param name="subAccountId"></param>
        /// <param name="transId"></param>
        /// <param name="isInternal"></param>
        /// <returns></returns>
        public bool Age_CustomerSelCheckBelonging(
            int custId,
            int currentCustId,
            int subAccountId,
            long transId,
            bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId }, 
                { "subaccountid", subAccountId }
            };

            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);
            parameters.Add("isactive", true);

            var outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging", parameters);

            return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal || Converter.ToBool(outputs["isactive"]));
        }

        /// <summary>
        ///     Check belonging in data warehouse DB.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="currentCustId"></param>
        /// <param name="transId"></param>
        /// <returns></returns>
        public bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId }
            };

            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);

            var outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_OLD", parameters);

            return (short)outputs["roleid"] > 0;
        }

        public bool Age_CustomerSelCheckBelonging_Multi(
            string custIds, 
            int currentCustId, 
            int subAccountId,
            bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId },
                { "custid", custIds },
                { "roleid", 0 },
                { "isactive", true }
            };

            var outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_MultiCust", parameters);

            return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal || Converter.ToBool(outputs["isactive"]));
        }

        #endregion

        #region ViewLogDependencies

        /// <summary>
        ///     Check if userName is belong to uplineId.
        /// </summary>
        /// <param name="userName">userName need to check</param>
        /// <param name="uplineId">Id of current login user</param>
        /// <param name="isSubAcc">return if userName is sub account</param>
        /// <returns>Return Id of userName</returns>
        public int Age_CustomerSelCheckBelonging_ByUsername(string userName, int uplineId, out bool isSubAcc)
        {
            var parameters = new Hashtable
            {
                { "uplineid", uplineId },
                { "username", userName },
                { "custid", 0 },
                { "issubacc", false }
            };

            var outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_ByUsername", parameters);

            isSubAcc = (bool)outputs["issubacc"];
            return (int)outputs["custid"];
        }

        /// <summary>
        ///     Check if userName is belong to uplineId.
        ///     Author: Jeff
        ///     Task: Search&Copy
        /// </summary>
        /// <param name="userName">userName need to check</param>
        /// <param name="uplineId">Id of current login user</param>
        /// <param name="isSubAcc">return if userName is sub account</param>
        /// <param name="roleId"></param>
        /// <returns>Return Id of userName</returns>
        public int Age_CustomerSelCheckBelonging_ByUsername(
            string userName, 
            int uplineId, 
            out bool isSubAcc,
            out int roleId)
        {
            var parameters = new Hashtable
            {
                { "uplineid", uplineId },
                { "username", userName },
                { "custid", 0 },
                { "issubacc", false },
                { "roleid", 0 }
            };

            var outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_ByUsername", parameters);

            isSubAcc = (bool)outputs["issubacc"];
            roleId = (int)outputs["roleid"];

            return (int)outputs["custid"];
        }

        /// <summary>
        ///     Check user whether has permission on a function or not.
        ///     Author: Lewin
        ///     Task: viewlog function
        /// </summary>
        /// <param name="userId">int-userid want to check his permission</param>
        /// <param name="funcId">int-functionid</param>
        /// <returns>bool</returns>
        public bool Age_CustomerSelCheckPermission(int userId, int funcId)
        {
            var hasPermission = false;

            var parms = new Hashtable
            {
                { "UserId", userId }, 
                { "FuncId", funcId }
            };

            var outputs = ExecSpHashtable("Age_CustomerSelCheckPermission", parms);

            hasPermission = (bool)outputs["Policy"];
            return hasPermission;
        }

        #endregion
    }
}