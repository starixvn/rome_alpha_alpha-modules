namespace Alpha.Auth.DataAccesses
{
    public interface IAuthorizationDataAccess
    {
        bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId);

        bool Age_CustomerSelCheckBelonging(
            int custId,
            int currentCustId,
            int subAccountId,
            long transId,
            bool isInternal);

        bool Age_CustomerSelCheckBelonging_Multi(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);
    }
}