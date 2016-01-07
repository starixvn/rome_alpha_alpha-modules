namespace Alpha.Auth.Services
{
    using Entities;

    public interface IAuthenticate
    {
        string Function { get; set; }

        AuthenticationType Policy { get; set; }

        bool AllowRead(string action);

        bool AllowUpdate(string action);

        bool IsBelong(int custId, string username, string groupName = null, bool getUplineInfo = true);
    }
}