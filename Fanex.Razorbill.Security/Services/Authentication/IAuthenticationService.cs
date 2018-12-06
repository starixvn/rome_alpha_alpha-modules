namespace Fanex.Razorbill.Security.Services.Authentication
{
    using Fanex.Razorbill.Security.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationService
    {
        bool CheckingClosedStatus();

        CustomerPermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse);

        bool CheckBelonging(int custId, int uplineId, byte uplineRoleId, long transId, bool isDataWarehouse);

        bool IsAuthorized();

        bool IsMultipleCustomersBelonging(string custIds);
    }
}
