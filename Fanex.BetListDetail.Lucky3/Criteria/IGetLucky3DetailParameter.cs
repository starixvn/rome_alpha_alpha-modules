namespace Fanex.BetListDetail.Lucky3.Criteria
{
    public interface IGetLucky3DetailParameter
    {
        long TransactionId { get; set; }

        long CustomerId { get; set; }

        bool IsValid();
    }
}