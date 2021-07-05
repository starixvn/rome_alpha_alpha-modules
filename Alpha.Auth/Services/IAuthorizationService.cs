namespace Alpha.Auth.Services
{
    using Models;

    public interface IAuthorizationService
    {
        AccountStatus GetAccountStatus();

        bool IsAccountActive();

        bool IsAuthorized(string url = null);

        bool IsBelonging();

        KickoutInfo IsKickedOut();
    }
}