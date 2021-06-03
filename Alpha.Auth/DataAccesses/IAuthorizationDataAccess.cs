namespace Alpha.Auth.DataAccesses
{
    using Models;

    public interface IAuthorizationDataAccess
    {
        AccountStatus GetAccountStatus(int customerId, int subAccountId);

        bool IsActive(int customerId, int subAccountId);

        string GetSubAccountPermission(int subAccountId);

        bool IsBelongingOld(int custId, int currentCustId, long transId);

        BelongingInfo IsBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        BelongingInfo IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        BelongingInfo IsMultipleCustomersBelonging(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);

        KickoutInfo IsKickedOut(int onlineUserId);
    }
}