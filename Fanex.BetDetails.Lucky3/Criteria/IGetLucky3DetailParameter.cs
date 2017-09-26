namespace Fanex.BetDetails.Lucky3.Criteria
{
    public interface IGetLucky3DetailParameter
    {
        long TransactionId { get; set; }

        long CustomerId { get; set; }

        byte LevelId { get; set; }

        bool IsValid();
    }
}