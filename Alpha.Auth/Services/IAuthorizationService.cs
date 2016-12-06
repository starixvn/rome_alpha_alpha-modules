namespace Alpha.Auth.Services
{
    public interface IAuthorizationService
    {
        bool IsAccountActive();

        bool IsAuthorized(string url = null);

        bool IsBelonging();

        bool IsKickedOut();
    }
}