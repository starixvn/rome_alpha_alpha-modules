namespace Fanex.BetListDetail.Lucky3.Criteria
{
    public class GetLucky3DetailParameter : ParameterBase
    {
        public long TransactionId { get; set; }

        public long CustomerId { get; set; }

        public override bool IsValid() => TransactionId > 0 && CustomerId > 0;
    }
}