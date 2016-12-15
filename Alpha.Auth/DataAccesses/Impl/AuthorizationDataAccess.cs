namespace Alpha.Auth.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;

    public class AuthorizationDataAccess : IAuthorizationDataAccess
    {
        public bool IsActive(int customerId, int subAccountId)
        {
            using (var objectDb = new ObjectDb("Age_CustomerStatusSel"))
            {
                dynamic inputParams = new Hashtable
                {
                    { "custid", customerId },
                    { "subaccid", subAccountId },
                    { "isactive", false }
                };

                objectDb.ExecuteNonQuery(inputParams);

                return Converter.ToBool(objectDb.GetOuputParams()["isactive"]);
            }
        }

        public string GetSubAccountPermission(int subAccountId)
        {
            dynamic inputParams = new Hashtable
            {
                { "subaccid", subAccountId },
                { "pageaccess", string.Empty }
            };

            using (var objectDb = new ObjectDb("Age_CustSubPageAccessSel"))
            {
                objectDb.ExecuteNonQuery(inputParams);

                return Converter.ToString2(objectDb.GetOuputParams()["pageaccess"]);
            }
        }

        public bool IsBelongingOld(int custId, int currentCustId, long transId)
        {
            dynamic parameters = new Hashtable
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
                objectDb.ExecuteNonQuery(parameters);
                var outputs = objectDb.GetOuputParams();
                return (short)outputs["roleid"] > 0;
            }
        }

        public bool IsBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            dynamic parameters = new Hashtable
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
                objectDb.ExecuteNonQuery(parameters);
                var outputs = objectDb.GetOuputParams();
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }
        }

        public bool IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            dynamic parameters = new Hashtable();

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
                objectDb.ExecuteNonQuery(parameters);
                var outputs = objectDb.GetOuputParams();
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal ? true : Converter.ToBool(outputs["isactive"]));
            }
        }

        public bool IsMultipleCustomersBelonging(string custIds, int currentCustId, int subAccountId, bool isInternal)
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
                return Converter.To<byte>(outputs["roleid"]) > 0 && (isInternal || Converter.ToBool(outputs["isactive"]));
            }
        }

        public bool IsKickedOut(int onlineUserId)
        {
            using (var objectDb = new ObjectDb("Age_ReCheckin"))
            {
                dynamic inputParams = new Hashtable
                {
                    { "on_userid", onlineUserId },
                    { "iskickedout", 0 }
                };

                objectDb.ExecuteNonQuery(inputParams);

                return Converter.ToBool(objectDb.GetOuputParams()["iskickedout"]);
            }
        }
    }
}