namespace Alpha.Web.Helpers
{
    using Alpha.Web.Configurations;
    using Alpha.Web.Models;
    using System;

    public static class AppSettingsHelper
    {
        public static bool GetUpdateCreditReplicateServer() => AppSettings.UpdateCreditBothDBCustAndDBAge;

        public static int GetInsertProfilePictureSiteMode() => AppSettings.InsertProfilePictureSiteMode;

        public static bool HideProductStatusBySites(string site) =>
             Array.Exists(
                 AppSettings.HideProductStatusSites,
                 disallowedSite => disallowedSite.Equals(site, StringComparison.OrdinalIgnoreCase));

        public static string GetProductConfigsFileRelativePath() => AppSettings.ProductConfigsFileRelativePath;

        public static string GetQRCodeMemberSiteLoginConfigsPath() => AppSettings.QRCodeMemberSiteLoginConfigsPath;

        public static bool GetAppSettingBooleanKey(AppSettingEnum settingKey)
        {
            switch (settingKey)
            {
                case AppSettingEnum.ShowOldCustomerList:
                    return AppSettings.ShowOldCustomerList;

                default:
                    return false;
            }
        }
    }
}
