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

    }
}