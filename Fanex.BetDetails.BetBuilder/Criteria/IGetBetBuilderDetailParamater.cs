namespace Fanex.BetDetails.BetBuilder.Criteria
{
    public interface IGetBetBuilderDetailParameter
    {
        long TransactionId { get; set; }

        long CustomerId { get; set; }

        string Status { get; }

        bool IsValid();
    }
}
