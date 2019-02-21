namespace Fanex.BetDetails.BetBuilder.Criteria
{
    public class GetBetBuilderDetailParameter : IGetBetBuilderDetailParameter
    {
        public long TransactionId { get; set; }

        public long CustomerId { get; set; }

        public string Status { get; set; }

        public bool IsValid() => TransactionId > 0 && CustomerId > 0;
    }
}
