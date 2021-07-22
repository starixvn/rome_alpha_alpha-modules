namespace Alpha.Auth.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Models;

    public class AuthorizationDataAccess : IAuthorizationDataAccess
    {
        public AccountStatus GetAccountStatus(int customerId, int subAccountId)
        {
            using (var db = new ObjectDb("Age_CustomerStatusSel"))
            {
                db.ExecuteNonQuery(new Hashtable
                {
                    ["custid"] = customerId,
                    ["subaccid"] = subAccountId,
                    ["isactive"] = false,
                    ["locked"] = 0
                });
                var ouputParams = db.GetOuputParams();

                return new AccountStatus(
                    Convert.ToBoolean(ouputParams["isactive"]),
                    Convert.ToByte(ouputParams["locked"]));
            }
        }

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

        public BelongingInfo IsBelonging(int custId, int currentCustId, int subAccountId, long transId)
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

                return new BelongingInfo
                {
                    RoleId = Converter.To<byte>(outputs["roleid"]),
                    IsActive = Converter.ToBool(outputs["isactive"])
                };
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

        public KickoutInfo IsKickedOut(int onlineUserId)
        {
            using (var objectDb = new ObjectDb("Age_ReCheckin"))
            {
                dynamic inputParams = new Hashtable
                {
                    { "on_userid", onlineUserId },
                    { "iskickedout", 0 },
                    { "kickoutreason", 0 }
                };

                objectDb.ExecuteNonQuery(inputParams);
                var outputParams = objectDb.GetOuputParams();

                return new KickoutInfo(Converter.ToBool(outputParams["iskickedout"]), Convert.ToByte(outputParams["kickoutreason"]));
            }
        }
    }
}