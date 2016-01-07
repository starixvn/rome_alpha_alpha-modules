namespace Alpha.Auth.Services
{
    using Alpha.Auth.Entities;

    public interface IAuthorizationService
    {
        bool CheckingClosedStatus();

        PermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse);

        bool CheckBelonging(int custId, long transId, bool isDataWarehouse);
    }
}