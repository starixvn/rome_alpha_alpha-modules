namespace Fanex.Razorbill.Security.Repositories.Authentication
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationRepository
    {
        bool CheckingCustomerStatus(int customerId, int subAccountId);

        string GetSubAccountPermission(int subAccountId);

        bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId);

        bool Age_CustomerSelCheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        bool Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        bool IsMultipleCustomersBelonging(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);
    }
}
