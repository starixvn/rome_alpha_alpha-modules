namespace Alpha.Auth.Services
{
    using Alpha.Auth.Models;

    public interface IAuthorizationService
    {
        bool IsAccountActive();

        PermissionResult CheckPermission();

        bool CheckBelonging();
    }
}