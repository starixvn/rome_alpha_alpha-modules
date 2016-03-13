namespace Alpha.Auth.Models
{
    using Sunplus.Agent.Model.Entities;

    public interface IRequestContext
    {
        string Url { get; }

        long TransId { get; }

        bool IsDataStoreInDataWarehouse { get; }

        int CustId { get; }

        IUserSession UserSession { get; }
    }
}