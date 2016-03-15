namespace Alpha.Auth.Services
{
    using Alpha.Auth.Models;

    public interface IAuthorizationService
    {
        bool IsAccountActive();

        bool CheckPermission(string url = null);

        bool CheckBelonging();
    }
}