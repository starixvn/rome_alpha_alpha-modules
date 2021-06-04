namespace Fanex.Razorbill.Security.Repositories.Authentication
{
    using System;
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Razorbill.Security.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public ClosedLockedStatus CheckClosedOrLockedStatus(int customerId, int subAccountId)
        {
            using (var db = new ObjectDb("Age_CustomerStatusSel"))
            {
                db.AssignParamValues(new Hashtable
                {
                    ["custid"] = customerId,
                    ["subaccid"] = subAccountId,
                    ["isactive"] = false,
                    ["locked"] = 0,
                });

                db.ExecuteNonQuery();
                var ouputParams = db.GetOuputParams();

                return new ClosedLockedStatus(
                    Convert.ToBoolean(ouputParams["isactive"]),
                    Convert.ToByte(ouputParams["locked"]));
            }
        }

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

        public BelongingResult Age_CustomerSelCheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
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

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                return new BelongingResult(Converter.To<byte>(outputs["roleid"]), isInternal || Converter.ToBool(outputs["isactive"]));
            }
        }

        public BelongingResult Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
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
                return new BelongingResult(Converter.To<byte>(outputs["roleid"]), isInternal || Converter.ToBool(outputs["isactive"]));
            }
        }

        public BelongingResult IsMultipleCustomersBelonging(string custIds, int currentCustId, int subAccountId, bool isInternal)
        {
            dynamic parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId },
                { "custid", custIds },
                { "roleid", 0 },
                { "isactive", true }
            };

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_MultiCust"))
            {
                objectDb.ExecuteNonQuery(parameters);
                var outputs = objectDb.GetOuputParams();
                return new BelongingResult(Converter.To<byte>(outputs["roleid"]), isInternal || Converter.ToBool(outputs["isactive"]));
            }
        }
    }
}
