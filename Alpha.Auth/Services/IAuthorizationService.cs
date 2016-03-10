namespace Alpha.Auth.Services
{
    using Alpha.Auth.Entities;

    public interface IAuthorizationService
    {
        bool CheckClosedStatus();

        PermissionResult CheckPermission();

        bool CheckBelonging();
    }
}