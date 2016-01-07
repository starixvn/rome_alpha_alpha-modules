namespace Alpha.Auth.Services
{
    using Alpha.Auth.Entities;

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