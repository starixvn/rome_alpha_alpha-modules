namespace Alpha.SmartPunter.Criteria
{
    public class SmartPunterSMAChecker
    {
        public SmartPunterSMAChecker(int customerId)
            : this(customerId, null)
        {
        }

        public SmartPunterSMAChecker(string username)
            : this(null, username)
        {
        }

        public SmartPunterSMAChecker(int? customerId, string username)
        {
            CustId = customerId;
            Username = username;
        }

        public int? CustId { get; }

        public string Username { get; }
    }
}