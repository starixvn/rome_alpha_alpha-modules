namespace Fanex.BetListDetail.Lucky3.Criteria
{
    public class GetLucky3DetailParameter : IGetLucky3DetailParameter
    {
        public long TransactionId { get; set; }

        public long CustomerId { get; set; }

        public bool IsValid() => TransactionId > 0 && CustomerId > 0;
    }
}