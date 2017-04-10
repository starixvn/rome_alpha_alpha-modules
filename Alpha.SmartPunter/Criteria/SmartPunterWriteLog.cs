namespace Alpha.SmartPunter.Criteria
{
    public class SmartPunterWriteLog
    {
        public SmartPunterWriteLog(
            string userNames,
            int actionId,
            bool isRestricted,
            int userId,
            string ip)
        {
            ListUserName = userNames;
            ActionId = actionId;
            IsRestricted = isRestricted;
            UserId = userId;
            IP = ip;
        }

        public string ListUserName { get; }

        public int ActionId { get; }

        public bool IsRestricted { get; }

        public int UserId { get; }

        public string IP { get; }
    }
}