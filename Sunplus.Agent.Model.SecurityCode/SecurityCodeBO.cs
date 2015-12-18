namespace Sunplus.Agent.Model.BL.MemberInfo.SecurityCode
{
    using System;
    using CasinoLibrary.SBAgent;
    using Fanex.Security;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.DL.MemberInfo.SecurityCode;

    [Obsolete("Should rename to Sunplus.Agent.Model.SecurityCode")]
    public class SecurityCodeBO : BOBase
    {
        private const string Md5 = "Md5";
        private const string Sha1 = "Sha1";
        private const string Sha256 = "Sha256";
        private const string SecurityCodePadding = "1qaw3+{:P)";

        public SecurityCodeBO(string netSessionId)
            : base(netSessionId)
        {
        }

        /// <summary>
        /// Salt value of security code with Md5
        /// </summary>
        /// <returns></returns>
        public string MD5Salt(int custId)
        {
            string plainText = custId.ToString();
            return Cryptography.ComputeHash(Md5, plainText, SecurityCodePadding);
        }

        /// <summary>
        /// Sha1 Security code to save into DB
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="safeCode"></param>
        /// <returns></returns>
        private string Sha1SecurityCode(int custId, string safeCode)
        {
            string salt = MD5Salt(custId);
            return Cryptography.ComputeHash(Sha1, safeCode, salt);
        }

        /// <summary>
        /// Change safe code of current customer or current sub acc
        /// </summary>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int ChangeSecurityCode(string safeCode, string ip, out string errMsg)
        {
            int errCode = 0;
            int custId = CurrentSession.CustId;

            string safeCodeHash = safeCode;
            if (CurrentSession.IsSubAcc)
            {
                SecurityCodeDAO dao = new SecurityCodeDAO();

                safeCodeHash = Sha1SecurityCode(CurrentSession.SubAccId, safeCodeHash);
                errCode = dao.Age_CustomerUpdSecurityCode_Sub(
                    custId,
                    CurrentSession.UserId,
                    CurrentSession.SubAccId,
                    safeCodeHash,
                    CurrentSession.LangKey,
                    ip,
                    out errMsg);
            }
            else
            {
                errCode = this.ChangeSecurityCode(custId, safeCodeHash, ip, out errMsg);
            }

            if (errCode == 0)
            {
                if (CurrentSession.SecurityCodeResult == Definitions.SCResult.FORCED_CHANGE_SC)
                {
                    CurrentSession.SecurityCodeResult = Definitions.SCResult.PROVIDED_SC_SUCCESS;
                }
                else if (CurrentSession.SecurityCodeResult == Definitions.SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION)
                {
                    CurrentSession.SecurityCode = safeCodeHash;
                    CurrentSession.SecurityCodeResult = Definitions.SCResult.PROVIDED_SC_SUCCESS;
                }
            }

            return errCode;
        }

        /// <summary>
        /// Change safe code of target customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int ChangeSecurityCode(int custId, string safeCode, string ip, out string errMsg)
        {
            string safeCodeHash = string.Empty;
            int errCode = 0;

            SecurityCodeDAO dao = new SecurityCodeDAO();

            if (CurrentSession.IsAgent && CurrentSession.CustId != custId)
            {
                safeCodeHash = Cryptography.CfsEnCode(safeCode);
            }
            else
            {
                //// change SC of owner
                safeCodeHash = Sha1SecurityCode(custId, safeCode);

                //// check new SC must be totally different from old SC
                string safeCodeDB = SelectSecurityCode();
                if (string.Equals(safeCodeHash, safeCodeDB))
                {
                    errCode = -1; 
                    errMsg = string.Empty;

                    return errCode;
                }
            }

            errCode = dao.Age_CustomerUpdSecurityCode(
                custId,
                CurrentSession.UserId,
                CurrentSession.SubAccId,
                safeCodeHash,
                CurrentSession.LangKey,
                ip,
                out errMsg);

            return errCode;
        }

        /// <summary>
        /// Check safe code of current customer in DB
        /// </summary>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int CheckSecurityCode(string safeCode, string ip, out string errMsg, bool isSessionSaving)
        {
            int errCode = -1;
            errMsg = string.Empty;

            //// get random key in Session
            string randomKey = CurrentSession.SecurityCodeToken;
            if (randomKey.Length != 0) 
            {
                //// has random key
                string safeCodeFromDB = SelectSecurityCode();
                //// Must not check SC is empty here

                string safeCodeFromDBHash = Cryptography.ComputeHash(Sha256, safeCodeFromDB, randomKey);
                bool status = string.Equals(safeCode, safeCodeFromDBHash);
                if (status)
                {
                    //// verify OK
                    //// we won't update SCResult if it equals FORCED_CHANGE_SC
                    if (CurrentSession.SecurityCodeResult != Definitions.SCResult.FORCED_CHANGE_SC)
                    {
                        CurrentSession.SecurityCodeResult = isSessionSaving
                            ? Definitions.SCResult.PROVIDED_SC_SUCCESS
                            : Definitions.SCResult.PROVIDED_SC;
                    }

                    //// reset random key
                    CurrentSession.SecurityCodeToken = string.Empty;
                }

                //// update status of Sec code
                errCode = UpdateSecurityCodeStatus(status, ip, out errMsg);
                if (errCode == 200)
                {
                    /* account is closed */
                    errCode = 2;
                    if (!CurrentSession.IsSubAcc)
                    {
                        new Customer().UpdateCustClosedFailSecurityCode(CurrentSession.CustId, true, ip);
                    }
                }
                else if (errCode != 0)
                {
                    /* wrong SC */
                    errCode = 9009;
                }
            }

            return errCode;
        }

        /// <summary>
        /// Update status when checking it
        /// </summary>
        /// <param name="status"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private int UpdateSecurityCodeStatus(bool status, string ip, out string errMsg)
        {
            SecurityCodeDAO dao = new SecurityCodeDAO();

            if (CurrentSession.IsSubAcc)
            {
                return dao.Age_CustomerUpdSecurityCodeStatus_Sub(
                    CurrentSession.CustId,
                    CurrentSession.SubAccId,
                    status,
                    CurrentSession.LangKey,
                    ip,
                    out errMsg);
            }
            else
            {
                return dao.Age_CustomerUpdSecurityCodeStatus(
                    CurrentSession.CustId,
                    status,
                    CurrentSession.LangKey,
                    ip,
                    out errMsg);
            }
        }

        /// <summary>
        /// Get security code from DB
        /// </summary>
        /// <returns></returns>
        private string SelectSecurityCode()
        {
            SecurityCodeDAO dao = new SecurityCodeDAO();

            return CurrentSession.IsSubAcc
                ? dao.Age_CustomerSelSecurityCode_Sub(CurrentSession.CustId, CurrentSession.SubAccId)
                : dao.Age_CustomerSelSecurityCode(CurrentSession.CustId);
        }

        /// <summary>
        /// Get security code from DB
        /// </summary>
        /// <returns></returns>
        public string SelectUplineSecurityCode()
        {
            SecurityCodeDAO dao = new SecurityCodeDAO();

            if (CurrentSession.IsSubAcc)
            {
                return dao.Age_CustomerSelSecurityCode(CurrentSession.CustId);
            }

            if (CurrentSession.IsAgent)
            {
                return dao.Age_CustomerSelSecurityCode(CurrentSession.MRecommendId);
            }

            if (CurrentSession.IsMaster)
            {
                return dao.Age_CustomerSelSecurityCode(CurrentSession.SRecommendId);
            }

            return dao.Age_CustomerSelSecurityCode(CurrentSession.CustId);
        }

        public void GenerateSecurityToken()
        {
            string token = Cryptography.RandomString(6);
            CurrentSession.SecurityCodeToken = token;
        }

        /// <summary>
        /// Reset security code of Sub-Account
        /// </summary>
        /// <param name="subAccId"></param>
        /// <param name="safeCode"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int ChangeSubAccountSecurityCode(int subAccId, string safeCode, string ip, out string errMsg)
        {
            int custId = CurrentSession.CustId;
            string safeCodeHash = Sha1SecurityCode(subAccId, safeCode);

            SecurityCodeDAO dao = new SecurityCodeDAO();
            int errCode = dao.Age_CustomerUpdSecurityCode_Sub2(
                subAccId,
                custId,
                CurrentSession.UserId,
                safeCodeHash,
                CurrentSession.LangKey,
                ip,
                out errMsg);

            return errCode;
        }
    }
}