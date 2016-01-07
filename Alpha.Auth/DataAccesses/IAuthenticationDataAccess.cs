namespace Alpha.Auth.DataAccesses
{
    public interface IAuthenticationDataAccess
    {
        bool IsActive(int customerId, int subAccountId);

        string GetSubAccountPermission(int subAccountId);

        bool IsBelongingOld(int custId, int currentCustId, long transId);

        bool IsBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        bool IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        bool IsMultipleCustomersBelonging(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);
    }
}