namespace Alpha.Auth.DataAccesses.Impl
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;

    public class AuthenticationDataAccess : IAuthenticationDataAccess
    {
        /// <summary>
        /// Checking customer status.
        /// </summary>
        /// <param name="customerId">Customer ID.</param>
        /// <param name="subAccountId">SUB ACC ID.</param>
        /// <returns>Return status: 1 - is active, 2 - is closed.</returns>
        public bool IsActive(int customerId, int subAccountId)
        {
            using (var objectDb = new ObjectDb("Age_CustomerStatusSel"))
            {
                var inputParams = new Hashtable
                {
                    { "custid", customerId },
                    { "subaccid", subAccountId },
                    { "isactive", false }
                };

                objectDb.AssignParamValues(inputParams);
                objectDb.ExecuteNonQuery();

                return Converter.ToBool(objectDb.GetOuputParams()["isactive"]);
            }
        }

        /// <summary>
        /// Get SUB ACCOUNT Permission.
        /// </summary>
        /// <param name="subAccountId">SUB ACC ID.</param>
        /// <returns>Return result.</returns>
        public string GetSubAccountPermission(int subAccountId)
        {
            var inputParams = new Hashtable
            {
                { "subaccid", subAccountId },
                { "pageaccess", string.Empty }
            };

            using (var objectDb = new ObjectDb("Age_CustSubPageAccessSel"))
            {
                objectDb.AssignParamValues(inputParams);
                objectDb.ExecuteNonQuery();

                return Converter.ToString2(objectDb.GetOuputParams()["pageaccess"]);
            }
        }

        public bool IsBelongingOld(int custId, int currentCustId, long transId)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "transid", transId },
                { "roleid", 0 }
            };

            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_OLD"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                return (short)outputs["roleid"] > 0;
            }
        }

        public bool IsBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId },
                { "transid", transId },
                { "roleid", 0 },
                { "isactive", true }
            };

            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }
        }

        public bool IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("sessioncustid", currentCustId);
            parameters.Add("subaccountid", subAccountId);
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);
            parameters.Add("isactive", true);

            using (var objectDb = new ObjectDb("Age_Rpt_CustomerConsolidatedGroup_CheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }
        }

        public bool IsMultipleCustomersBelonging(string custIds, int currentCustId, int subAccountId, bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId },
                { "custid", custIds },
                { "roleid", 0 },
                { "isactive", true }
            };

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_MultiCust"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal || Converter.ToBool(outputs["isactive"]));
            }
        }
    }
}