namespace Alpha.SmartPunter.Criteria
{
    public class SmartPunterSMACheckerWriteLog : SmartPunterWriteLog
    {
        public SmartPunterSMACheckerWriteLog(
            string userNames,
            int actionId,
            bool isRestricted,
            int userId,
            string ip)
            : base(userNames, actionId, isRestricted, userId, ip)
        {
        }
    }
}