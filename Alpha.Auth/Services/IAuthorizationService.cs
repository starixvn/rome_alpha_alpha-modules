namespace Alpha.Auth.Services
{
    using Alpha.Auth.Entities;

    public interface IAuthorizationService
    {
        bool IsAccountActive();

        PermissionResult CheckPermission();

        bool CheckBelonging();
    }
}