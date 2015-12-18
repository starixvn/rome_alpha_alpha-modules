namespace Sunplus.Agent.Model.DL.MemberInfo.SecurityCode
{
    using System;
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.DL.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.SecurityCode")]
    public class SecurityCodeDAO : DAOBase
    {
        /// <summary>
        /// Change safe code of current customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="safeCode"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Age_CustomerUpdSecurityCode(
            int custId,
            int userId,
            int subAccId,
            string safeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "userid", userId },
                { "subaccid", subAccId },
                { "securitycode", safeCode },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerUpdSecurityCode", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_CustomerUpdSecurityCode_Sub(
            int custId,
            int userId,
            int subAccId,
            string safeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "userid", userId },
                { "subaccid", subAccId },
                { "securitycode", safeCode },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerUpdSecurityCode_Sub", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_CustomerUpdSecurityCode_Sub2(
            int subAccId,
            int custId,
            int userId,
            string safeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "userid", userId },
                { "subaccid", subAccId },
                { "securitycode", safeCode },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerUpdSecurityCode_Sub2", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        /// Check safe code of current customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="safeCode"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Age_CustomerCheckSafeCode(
            int custId,
            string safeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "securitycode", Fanex.Common.Cryptography.CreateHash(safeCode, custId.ToString()) },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerCheckSafeCode", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        /// Get security code of current customer
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public string Age_CustomerSelSecurityCode(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);

            string safeCode = string.Empty;
            DataTable safeCodeTable = ExecSpTable("Age_CustomerSelSecurityCode", parameters);
            if (safeCodeTable != null && safeCodeTable.Rows.Count > 0)
            {
                safeCode = (string)safeCodeTable.Rows[0][0].ToString();
            }

            return safeCode;
        }

        public string Age_CustomerSelSecurityCode_Sub(int custId, int subAccId)
        {
            Hashtable parameters = new Hashtable { { "custid", custId }, { "subaccid", subAccId } };

            string safeCode = string.Empty;
            DataTable safeCodeTable = ExecSpTable("Age_CustomerSelSecurityCode_Sub", parameters);
            if (safeCodeTable != null && safeCodeTable.Rows.Count > 0)
            {
                safeCode = (string)safeCodeTable.Rows[0][0].ToString();
            }

            return safeCode;
        }

        public int Age_CustomerUpdSecurityCodeStatus(
            int custId,
            bool validSafeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "checkedsecuritycode", validSafeCode },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerUpdSecurityCodeStatus", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        public int Age_CustomerUpdSecurityCodeStatus_Sub(
            int custId,
            int subAccId,
            bool validSafeCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "subaccid", subAccId },
                { "checkedsecuritycode", validSafeCode },
                { "language", language },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            Hashtable outputs = ExecSpHashtable("Age_CustomerUpdSecurityCodeStatus_Sub", parameters);
            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }
    }
}