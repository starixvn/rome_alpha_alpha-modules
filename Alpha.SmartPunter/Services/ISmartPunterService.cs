namespace Alpha.SmartPunter.Services
{
    using Models;

    public interface ISmartPunterService
    {
        bool CheckInternalAdmin(int customerId);

        SMACustomer CheckSmartPunter(int customerId);

        void WriteLog(string userNames, bool isRestricted, int userId, string ip);

        void CheckSmartPunterAndWriteLog(string userNames, bool isRestricted, int userId, string ip);

        void WriteLogNoData(int? customerId, bool isRestricted, int userId, string ip);

        bool HasSmartPunterPermission(int loggedInUserId);
    }
}