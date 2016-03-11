namespace Alpha.Auth.Models
{
    public interface IRequestContext
    {
        string Url { get; }

        long TransId { get; }

        bool IsDataStoreInDataWarehouse { get; }

        int CustId { get; }
    }
}