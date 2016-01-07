namespace Alpha.Auth.DataAccesses.Impl
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationDataAccess : IAuthenticationDataAccess
    {
        /// <summary>
        /// Checking customer status.
        /// </summary>
        /// <param name="customerId">Customer ID.</param>
        /// <param name="subAccountId">SUB ACC ID.</param>
        /// <returns>Return status: 1 - is active, 2 - is closed.</returns>
        public bool CheckingCustomerStatus(int customerId, int subAccountId)
        {
            using (var objectDb = new ObjectDb("Age_CustomerStatusSel"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", customerId);
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("isactive", false);

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
            using (var objectDb = new ObjectDb("Age_CustSubPageAccessSel"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("pageaccess", string.Empty);

                objectDb.AssignParamValues(inputParams);
                objectDb.ExecuteNonQuery();

                return Converter.ToString2(objectDb.GetOuputParams()["pageaccess"]);
            }
        }

        public bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("sessioncustid", currentCustId);
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);

            var isBelonging = false;
            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_OLD"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                isBelonging = (short)outputs["roleid"] > 0;
            }

            return isBelonging;
        }

        public bool Age_CustomerSelCheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
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

            var isBelonging = false;
            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                isBelonging = Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }

            return isBelonging;
        }

        public bool Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
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

            var isBelonging = false;
            using (var objectDb = new ObjectDb("Age_Rpt_CustomerConsolidatedGroup_CheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                isBelonging = Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }

            return isBelonging;
        }
    }
}