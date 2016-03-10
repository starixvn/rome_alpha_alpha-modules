namespace Alpha.Auth.Models
{
    public interface IRequestContext
    {
        string Url { get; }

        bool IsSessionNull { get; }

        bool IsIgnore { get; }

        long TransId { get; }

        bool IsDataStoreInDataWarehouse { get; }

        int CustId { get; }
    }
}