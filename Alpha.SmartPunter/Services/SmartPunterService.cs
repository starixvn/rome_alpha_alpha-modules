namespace Alpha.SmartPunter.Services
{
    using Criteria;
    using DataAccesses;
    using Models;

    public class SmartPunterService : ISmartPunterService
    {
        private const int LogActionId = 31008;
        private const string SmartPunterFunctionName = "[SmartPunter]";

        private readonly ISmartPunterDataAccess smartPunterDataAccess;

        public SmartPunterService(ISmartPunterDataAccess smartPunterDataAccess)
        {
            this.smartPunterDataAccess = smartPunterDataAccess;
        }

        public bool CheckInternalAdmin(int customerId)
            => smartPunterDataAccess.CheckInternalAdmin(new InternalAdminChecker(customerId));

        public SMACustomer CheckSmartPunter(int customerId)
            => smartPunterDataAccess.CheckSmartPunter(new SmartPunterSMAChecker(customerId));

        public void WriteLog(string userNames, bool isRestricted, int userId, string ip)
         => smartPunterDataAccess.WriteLog(new SmartPunterWriteLog(
                userNames,
                LogActionId,
                isRestricted,
                userId,
                ip));

        public void CheckSmartPunterAndWriteLog(string userNames, bool isRestricted, int userId, string ip)
            => smartPunterDataAccess.CheckSmartPunterAndWriteLog(new SmartPunterSMACheckerWriteLog(
                userNames,
                LogActionId,
                isRestricted,
                userId,
                ip));

        public void WriteLogNoData(int? customerId, bool isRestricted, int userId, string ip)
            => smartPunterDataAccess.WriteLogNoData(new SmartPunterWriteLogNoData(
                customerId,
                LogActionId,
                isRestricted,
                userId,
                ip));

        public bool HasSmartPunterPermission(int loggedInUserId)
            => smartPunterDataAccess.CheckPermission(new NAPUserPermission(loggedInUserId), SmartPunterFunctionName);
    }
}