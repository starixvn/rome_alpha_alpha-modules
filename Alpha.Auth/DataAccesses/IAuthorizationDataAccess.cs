namespace Alpha.Auth.DataAccesses
{
    using Alpha.Auth.Models;

    public interface IAuthorizationDataAccess
    {
        bool IsActive(int customerId, int subAccountId);

        string GetSubAccountPermission(int subAccountId);

        bool IsBelongingOld(int custId, int currentCustId, long transId);

        BelongingInfo IsBelonging(int custId, int currentCustId, int subAccountId, long transId);

        bool IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        bool IsMultipleCustomersBelonging(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);

        bool IsKickedOut(int onlineUserId);
    }
}