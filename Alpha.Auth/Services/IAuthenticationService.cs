namespace Fanex.Razorbill.Security.Services.Authentication
{
    using Fanex.Razorbill.Security.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationService
    {
        bool CheckingClosedStatus();

        PermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse);

        bool CheckBelonging(int custId, long transId, bool isDataWarehouse);
    }
}
