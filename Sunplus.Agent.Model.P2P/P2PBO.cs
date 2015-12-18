namespace Sunplus.Agent.Model.BL.MemberInfo.P2P
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.MemberInfo.P2P;

    [Obsolete("Should rename to Sunplus.Agent.Model.P2P")]
    public class P2PBO : BOBase
    {
        private P2PDAO dao = new P2PDAO();
        private AuthorizationBO auth;

        public P2PBO(string netSessionId)
            : base(netSessionId)
        {
            this.auth = new AuthorizationBO(netSessionId);
        }

        #region gets infos

        public DataSet GetP2PInfo(int custId, int roleId, out int errCode, out string errMsg)
        {
            if (roleId == 2)
            {
                return this.dao.Acc_CustomerSelP2PAgent_All(custId, out errCode, out errMsg);
            }
            else if (roleId == 3)
            {
                return this.dao.Acc_CustomerSelP2PMaster_All(custId, out errCode, out errMsg);
            }
            else if (roleId == 4)
            {
                return this.dao.Acc_CustomerSelP2PSuper_All(custId, out errCode, out errMsg);
            }
            else if (roleId == 1)
            {
                return this.dao.Acc_CustomerSelP2PMember_All(custId, out errCode, out errMsg);
            }
            else 
            {
                errCode = -2;
                errMsg = string.Empty;

                return null;
            }
        }

        public DataTable GetP2PComm(int custId, int roleId)
        {
            if (roleId == 4)
            {
                return this.dao.Age_CustomerSelP2PComm_SAg(custId);
            }
            else if (roleId == 3)
            {
                return this.dao.Age_CustomerSelP2PComm_MAg(custId);
            }
            else if (roleId == 2)
            {
                return this.dao.Age_CustomerSelP2PComm_Ag(custId);
            }

            return null;
        }
        #endregion

        #region updates comm
        public void UpdateP2PComm(
                int custId, 
                int roleId,
                decimal discount_1,
                decimal discount_2,
                int subAccID,
                int userID,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            var disabled = this.IsDisabledAccount(custId, out errMsg);
            errCode = -999;

            if (disabled) 
            {
                return;
            }

            if (roleId == 2)
            {
                this.dao.Acc_CustomerUpdAgent_P2PCommission(
                    custId,
                    discount_1,
                    discount_2,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else if (roleId == 3)
            {
                this.dao.Acc_CustomerUpdMaster_P2PCommission(
                    custId,
                    discount_1,
                    discount_2,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else if (roleId == 4)
            {
                this.dao.Acc_CustomerUpdP2PSuper_Commission(
                    custId,
                    discount_1,
                    discount_2,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else
            {
                this.dao.Acc_CustomerUpdMember_P2PCommission(
                    custId,
                    discount_1,
                    discount_2,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
        }

        public void UpdateP2PComm(
                IList<int> custIds,
                int roleId,
                decimal discount_1,
                decimal discount_2,
                IList<int> subAccIDs,
                IList<int> userIDs,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            var tmpCode = new List<int>();
            var tmpMsg = new List<string>();
            var tmpCustId = new List<int>();

            foreach (int custId in custIds)
            {
                var errCode = -999;
                var errMsg = string.Empty;
                var disabled = this.IsDisabledAccount(custId, out errMsg);

                if (disabled)
                {
                    tmpCode.Add(errCode);
                    tmpMsg.Add(errMsg);
                }
                else
                {
                    tmpCustId.Add(custId);
                }
            }

            if (roleId == 2)
            {
                this.dao.Acc_CustomerUpdP2PAgent_Commission(
                    tmpCustId,
                    discount_1,
                    discount_2,
                    subAccIDs,
                    userIDs,
                    language,
                    ip,
                    out errCodes,
                    out errMsgs);
            }
            else if (roleId == 3)
            {
                this.dao.Acc_CustomerUpdP2PMaster_Commission(
                    tmpCustId,
                    discount_1,
                    discount_2,
                    subAccIDs,
                    userIDs,
                    language,
                    ip,
                    out errCodes,
                    out errMsgs);
            }
            else if (roleId == 4)
            {
                this.dao.Acc_CustomerUpdP2PSuper_Commission(
                    tmpCustId,
                    discount_1,
                    discount_2,
                    subAccIDs,
                    userIDs,
                    language,
                    ip,
                    out errCodes,
                    out errMsgs);
            }
            else
            {
                this.dao.Acc_CustomerUpdP2PMember_Commission(
                    tmpCustId,
                    discount_1,
                    discount_2,
                    subAccIDs,
                    userIDs,
                    language,
                    ip,
                    out errCodes,
                    out errMsgs);
            }

            for (var i = 0; i < tmpCode.Count(); i++)
            {
                errCodes.Add(tmpCode.ElementAt(i));
                errMsgs.Add(tmpMsg.ElementAt(i));
            }
        }       
        #endregion

        #region updates game setting
        public void UpdateP2PGameSetting(
                int custId,
                int subAccId,
                string ip,
                int userId,
                string language,
                IList<int> bettypeIds,
                string vals,
                int roleId,
                out IList<int> errCodes, 
                out IList<string> errMsgs)
        {
            var errMsg = string.Empty;
            var disabled = this.IsDisabledAccount(custId, out errMsg);

            if (disabled)
            {
                errCodes = new List<int>();
                errMsgs = new List<string>();

                errCodes.Add(-999);
                errMsgs.Add(errMsg);

                return;
            }

            if (roleId == Convert.ToInt32(UserTypes.Super))
            {
                this.dao.Acc_CustomerUpdSuper_P2PGameSetting(custId, subAccId, ip, userId, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else if (roleId == Convert.ToInt32(UserTypes.Master))
            {
                this.dao.Acc_CustomerUpdMaster_P2PGameSetting(custId, subAccId, ip, userId, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else if (roleId == Convert.ToInt32(UserTypes.Agent))
            {
                this.dao.Acc_CustomerUpdAgent_P2PGameSetting(custId, subAccId, ip, userId, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else
            {
                this.dao.Acc_CustomerUpdMember_P2PGameSetting(custId, subAccId, ip, userId, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
        }

        public void UpdateP2PGameSetting(
                IList<int> custIds,
                IList<int> subAccIds,
                string ip,
                IList<int> userIds,
                string language,
                IList<int> bettypeIds,
                string vals,
                int roleId,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            var tmpCode = new List<int>();
            var tmpMsg = new List<string>();
            var tmpCustId = new List<int>();

            foreach (int custId in custIds)
            {
                var errCode = -999;
                var errMsg = string.Empty;
                var disabled = this.IsDisabledAccount(custId, out errMsg);

                if (disabled)
                {
                    tmpCode.Add(errCode);
                    tmpCode.Add(errCode);
                    tmpMsg.Add(errMsg);
                    tmpMsg.Add(errMsg);
                }
                else
                {
                    tmpCustId.Add(custId);
                }
            }

            if (roleId == Convert.ToInt32(UserTypes.Super))
            {
                this.dao.Acc_CustomerUpdSuper_P2PGameSetting(tmpCustId, subAccIds, ip, userIds, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else if (roleId == Convert.ToInt32(UserTypes.Master))
            {
                this.dao.Acc_CustomerUpdMaster_P2PGameSetting(tmpCustId, subAccIds, ip, userIds, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else if (roleId == Convert.ToInt32(UserTypes.Agent))
            {
                this.dao.Acc_CustomerUpdAgent_P2PGameSetting(tmpCustId, subAccIds, ip, userIds, language, bettypeIds, vals, out errCodes, out errMsgs);
            }
            else
            {
                this.dao.Acc_CustomerUpdMember_P2PGameSetting(tmpCustId, subAccIds, ip, userIds, language, bettypeIds, vals, out errCodes, out errMsgs);
            }

            for (var i = 0; i < tmpCode.Count(); i++)
            {
                errCodes.Add(tmpCode.ElementAt(i));
                errMsgs.Add(tmpMsg.ElementAt(i));
            }
        }

        public void UpdateP2PGameSetting(
                int custId,
                int roleId,
                bool stakeLevel1_1001,
                bool stakeLevel2_1001,
                bool stakeLevel3_1001,
                bool stakeLevel4_1001,
                bool stakeLevel5_1001,
                bool stakeLevel1_1002,
                bool stakeLevel2_1002,
                bool stakeLevel3_1002,
                bool stakeLevel4_1002,
                bool stakeLevel5_1002,
                int subAccID,
                int userID,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            if (roleId == 2)
            {
                this.dao.Acc_CustomerUpdAgent_P2PGameSetting(
                    custId,
                    stakeLevel1_1001,
                    stakeLevel2_1001,
                    stakeLevel3_1001,
                    stakeLevel4_1001,
                    stakeLevel5_1001,
                    stakeLevel1_1002,
                    stakeLevel2_1002,
                    stakeLevel3_1002,
                    stakeLevel4_1002,
                    stakeLevel5_1002,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else if (roleId == 3)
            {
                this.dao.Acc_CustomerUpdMaster_P2PGameSetting(
                    custId,
                    stakeLevel1_1001,
                    stakeLevel2_1001,
                    stakeLevel3_1001,
                    stakeLevel4_1001,
                    stakeLevel5_1001,
                    stakeLevel1_1002,
                    stakeLevel2_1002,
                    stakeLevel3_1002,
                    stakeLevel4_1002,
                    stakeLevel5_1002,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else if (roleId == 4)
            {
                this.dao.Acc_CustomerUpdP2PSuper_GameSetting(
                    custId,
                    stakeLevel1_1001,
                    stakeLevel2_1001,
                    stakeLevel3_1001,
                    stakeLevel4_1001,
                    stakeLevel5_1001,
                    stakeLevel1_1002,
                    stakeLevel2_1002,
                    stakeLevel3_1002,
                    stakeLevel4_1002,
                    stakeLevel5_1002,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }
            else
            {
                this.dao.Acc_CustomerUpdMember_P2PGameSetting(
                    custId,
                    stakeLevel1_1001,
                    stakeLevel2_1001,
                    stakeLevel3_1001,
                    stakeLevel4_1001,
                    stakeLevel5_1001,
                    stakeLevel1_1002,
                    stakeLevel2_1002,
                    stakeLevel3_1002,
                    stakeLevel4_1002,
                    stakeLevel5_1002,
                    subAccID,
                    userID,
                    language,
                    ip,
                    out errCode,
                    out errMsg);
            }            
        }
        #endregion

        #region updates max transfer
        public void UpdateP2PMaxTransfer(
                int custId,
                int roleId,
                decimal maxTransfer,
                bool isDailyAutoReset,
                int subAccID,
                int userID,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            var disabled = this.IsDisabledAccount(custId, out errMsg);
            errCode = -999;

            if (disabled)
            {
                return;
            }

            if (roleId == 2)
            {
                this.dao.Acc_CustomerUpdAgent_P2PMaxTransfer(
                       custId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccID,
                       userID,
                       language,
                       ip,
                       out errCode,
                       out errMsg);
            }
            else if (roleId == 3)
            {
                this.dao.Acc_CustomerUpdMaster_P2PMaxTransfer(
                       custId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccID,
                       userID,
                       language,
                       ip,
                       out errCode,
                       out errMsg);
            }
            else if (roleId == 4)
            {
                this.dao.Acc_CustomerUpdP2PSuper_MaxTransfer(
                       custId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccID,
                       userID,
                       language,
                       ip,
                       out errCode,
                       out errMsg);
            }
            else
            {
                this.dao.Acc_CustomerUpdMember_P2PMaxTransfer(
                       custId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccID,
                       userID,
                       language,
                       ip,
                       out errCode,
                       out errMsg);
            }            
        }

        public void UpdateP2PMaxTransfer(
                IList<int> custIds,
                int roleId,
                decimal maxTransfer,
                bool isDailyAutoReset,
                IList<int> subAccIDs,
                IList<int> userIDs,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            var tmpCode = new List<int>();
            var tmpMsg = new List<string>();
            var tmpCustId = new List<int>();

            foreach (int custId in custIds)
            {
                var errCode = -999;
                var errMsg = string.Empty;
                var disabled = this.IsDisabledAccount(custId, out errMsg);

                if (disabled)
                {
                    tmpCode.Add(errCode);
                    tmpMsg.Add(errMsg);
                }
                else
                {
                    tmpCustId.Add(custId);
                }
            }

            if (roleId == 2)
            {
                this.dao.Acc_CustomerUpdP2PAgent_MaxTransfer(
                       tmpCustId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccIDs,
                       userIDs,
                       language,
                       ip,
                       out errCodes,
                       out errMsgs);
            }
            else if (roleId == 3)
            {
                this.dao.Acc_CustomerUpdP2PMaster_MaxTransfer(
                       tmpCustId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccIDs,
                       userIDs,
                       language,
                       ip,
                       out errCodes,
                       out errMsgs);
            }
            else if (roleId == 4)
            {
                this.dao.Acc_CustomerUpdP2PSuper_MaxTransfer(
                       tmpCustId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccIDs,
                       userIDs,
                       language,
                       ip,
                       out errCodes,
                       out errMsgs);
            }
            else
            {
                this.dao.Acc_CustomerUpdP2PMember_MaxTransfer(
                       tmpCustId,
                       maxTransfer,
                       isDailyAutoReset,
                       subAccIDs,
                       userIDs,
                       language,
                       ip,
                       out errCodes,
                       out errMsgs);
            }

            for (var i = 0; i < tmpCode.Count(); i++)
            {
                errCodes.Add(tmpCode.ElementAt(i));
                errMsgs.Add(tmpMsg.ElementAt(i));
            }
        }
        #endregion

        #region Check disable account
        public bool IsDisabledAccount(int custId, out string errMsg)
        {
            return !this.auth.IsCustomerEnable(custId, out errMsg);
        }
        #endregion
    }
}
