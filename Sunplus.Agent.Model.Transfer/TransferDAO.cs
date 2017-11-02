namespace Sunplus.Agent.Model.DL.Transfer
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.DL.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.Transfer")]
    public class TransferDAO : DAOBase
    {
        public int Age_Transfer_Ins_Ag(string fromCustId, string toUserName, int creatorId, int subAccId, string ip, double amt, bool isFullTransfer, string superIdStransferTotal, string language, bool isAllDayBalTransfer, out string errMsg)
        {
            // isYes=1 --> Only Yesterday
            // isYes=0 --> Get All
            var parameters = new Hashtable
            {
                { "superidstransfertotal", superIdStransferTotal },
                { "fromcustid", fromCustId },
                { "tousername", toUserName },
                { "amount", amt },
                { "creator", creatorId },
                { "language", language },
                { "subaccid", subAccId },
                { "ip", ip },
                { "isalldaybaltransfer", isAllDayBalTransfer }
            };
            //// AppSession.UserID : UserID of Admin Login
            var outputs = new Hashtable();
            try
            {
                outputs = ExecSpHashtable("Age_Transfer_Ins_Ag", parameters);
            }
            catch (FormatException ex)
            {
                ExceptionHandler.Handle(ex, TraceEventType.Information);
                errMsg = "Oops... you are here because of a system error, please notify the administrators!";
                return -1;
            }

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_Transfer_Ins_MAg(string toCustIds, int creatorId, int subAccId, int masterId, string ip, double amt, bool isFullTransfer, string status, string superIdStransferTotal, string language, out string errMsg)
        {
            // isYes=1 --> Only Yesterday
            // isYes=0 --> Get All
            var parameters = new Hashtable
            {
                { "superidstransfertotal", superIdStransferTotal },
                { "tocustids", toCustIds },
                { "creator", creatorId },
                { "fulltransfer", isFullTransfer },
                { "language", language },
                { "subaccid", subAccId },
                { "ip", ip },
                { "amttrans", amt },
                { "masterid", masterId },
                { "custstatus", status }
            };

            var outputs = ExecSpHashtable("Age_Transfer_Ins_MAg", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_Transfer_Ins_SAg(string toCustIds, int creatorId, int subAccId, int superId, string ip, double amt, bool isFullTransfer, string status, string superIdStransferTotal, string language, out string errMsg)
        {
            //// isYes=1 --> Only Yesterday
            //// isYes=0 --> Get All
            var parameters = new Hashtable
            {
                { "superidstransfertotal", superIdStransferTotal },
                { "tocustids", toCustIds },
                { "creator", creatorId },
                { "fulltransfer", isFullTransfer },
                { "language", language },
                { "subaccid", subAccId },
                { "ip", ip },
                { "amttrans", amt },
                { "sagid", superId },
                { "custstatus", status }
            };

            var outputs = ExecSpHashtable("Age_Transfer_Ins_SAg", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_Transfer_InsClearBalance_Ag(string toCustIds, int subAccId, int creatorId, int fromCustId, string ip, string status, string language, string superIdStransferTotal, bool isAllDayBalTransfer, out string errMsg)
        {
            var parameters = new Hashtable
            {
                { "fromcustid", fromCustId },
                { "creator", creatorId },
                { "tocustids", toCustIds },
                { "language", language },
                { "subaccid", subAccId },
                { "ip", ip },
                { "custstatus", status },
                { "superidstransfertotal", superIdStransferTotal },
                { "isalldaybaltransfer", isAllDayBalTransfer }
            };

            var outputs = ExecSpHashtable("Age_Transfer_InsClearBalance_Ag", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is agent</returns>
        public DataTable Age_Transfer_SelList_Ag(
            int custid,
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance,
            bool isAllowedTotalBalTrans)
        {
            //// Function list data transfer of Agent
            var htb = new Hashtable
            {
                { "custid", custid },
                { "pageindex", pageIndex },
                { "pagesize", pageSize },
                { "username", userName },
                { "isyesterdaybalance", isYesterdayBalance },
                { "isallowtotalbalance", isAllowedTotalBalTrans },
                { "closed", closed },
                { "suspended", suspended },
                { "enabled", enabled }
            };

            var rs = ExecSp("Age_Transfer_SelList_Ag", htb);
            totalPages = (int)rs.Outputs["totalpages"];

            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is master</returns>
        public DataTable Age_Transfer_SelList_MAg(
            int custid,
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance,
            bool isAllowedTotalBalTrans)
        {
            //// Function list data transfer of Agent
            var htb = new Hashtable
            {
                { "masterid", custid },
                { "pageindex", pageIndex },
                { "pagesize", pageSize },
                { "username", userName },
                { "isyesterdaybalance", isYesterdayBalance },
                { "isallowtotalbalance", isAllowedTotalBalTrans },
                { "closed", closed },
                { "suspended", suspended },
                { "enabled", enabled }
            };

            var rs = ExecSp("Age_Transfer_SelList_MAg", htb);
            totalPages = (int)rs.Outputs["totalpages"];

            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is super</returns>
        public DataTable Age_Transfer_SelList_SAg(
            int custid,
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance,
            bool isAllowedTotalBalTrans)
        {
            //// Function list data transfer of Agent
            var htb = new Hashtable
            {
                { "sagid", custid },
                { "pageindex", pageIndex },
                { "pagesize", pageSize },
                { "username", userName },
                { "isyesterdaybalance", isYesterdayBalance },
                { "isallowtotalbalance", isAllowedTotalBalTrans },
                { "closed", closed },
                { "suspended", suspended },
                { "enabled", enabled }
            };

            var rs = ExecSp("Age_Transfer_SelList_SAg", htb);
            totalPages = (int)rs.Outputs["totalpages"];

            return rs.ResultSet.Tables[0];
        }

        public Hashtable Age_Transfer_SelMemberInfo_Yesterday(string custid, bool isAllowedTotalBalanceTransfer)
        {
            var htb = new Hashtable
            {
                { "custid", custid },
                { "isallowtotalbalance", isAllowedTotalBalanceTransfer },
                { "username", string.Empty },
                { "synccasino", 0 },
                { "disablecasino", 0 },
                { "cashbalance", 0 },
                { "oustanding", 0 },
                { "givencredit", 0 },
                { "betcredit", 0 },
                { "lastlogin", DateTime.Now },
                { "balance", 0 },
                { "maxtransdate", DateTime.Now },
                { "futurebetcredit", 0 }
            };

            var rs = ExecSp("Age_Transfer_SelMemberInfo_Yesterday", htb);

            return rs.Outputs;
        }

        // start Jeff add Check for existing transfer 081111
        public DataTable Age_Transfer_CheckExisting(string custid)
        {
            var htb = new Hashtable { { "custid", custid } };

            var rs = ExecSp("Age_Transfer_CheckExisting", htb);

            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        /// Get option transfer Condition
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public string Acc_CustomerSelTransferOption(int custId)
        {
            return Acc_CustomerSelTransferOption(custId, string.Empty);
        }

        public string Acc_CustomerSelTransferOption(int custId, string transferOption)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "transferoption", transferOption }
            };

            var outputs = ExecSpHashtable("Acc_CustomerSelTransferOption", parameters);

            return (string)outputs["transferoption"];
        }

        public Hashtable Acc_CustomerUpdTransferOption(
            string custId,
            string transferOption,
            string userId,
            int subAccId,
            string langKey,
            string ip)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "transferoption", transferOption },
                { "userid", userId },
                { "subaccid", subAccId },
                { "language", langKey },
                { "ip", ip },
                { "returnerrcode", 1 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            return ExecSpHashtable("Acc_CustomerUpdTransferOption", parameters);
        }
    }
}