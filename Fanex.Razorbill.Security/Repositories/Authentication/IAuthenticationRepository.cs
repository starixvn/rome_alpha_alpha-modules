using Fanex.Razorbill.Security.Entities;

namespace Fanex.Razorbill.Security.Repositories.Authentication
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationRepository
    {
        ClosedLockedStatus CheckClosedOrLockedStatus(int customerId, int subAccountId);

        bool CheckingCustomerStatus(int customerId, int subAccountId);

        string GetSubAccountPermission(int subAccountId);

        bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId);

        BelongingResult Age_CustomerSelCheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        BelongingResult Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal);

        BelongingResult IsMultipleCustomersBelonging(
            string custIds,
            int currentCustId,
            int subAccountId,
            bool isInternal);
    }
}
