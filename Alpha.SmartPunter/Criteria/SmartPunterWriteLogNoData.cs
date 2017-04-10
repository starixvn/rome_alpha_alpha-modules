namespace Alpha.SmartPunter.Criteria
{
    public class SmartPunterWriteLogNoData
    {
        public SmartPunterWriteLogNoData(
            int? customerId,
            int actionId,
            bool isRestricted,
            int userId,
            string ip)
        {
            CustId = customerId;
            ActionId = actionId;
            IsRestricted = isRestricted;
            UserId = userId;
            IP = ip;
        }

        public int? CustId { get; }

        public int ActionId { get; }

        public bool IsRestricted { get; }

        public int UserId { get; }

        public string IP { get; }
    }
}