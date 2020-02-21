namespace Alpha.Web.Helpers
{
    using Alpha.Web.Configurations;
    using System;

    public static class AppSettingsHelper
    {
        public static bool GetUpdateCreditReplicateServer() => AppSettings.UpdateCreditBothDBCustAndDBAge;

        public static int GetInsertProfilePictureSiteMode() => AppSettings.InsertProfilePictureSiteMode;

        public static bool HideProductStatusBySites(string site) =>
             Array.Exists(
                 AppSettings.HideProductStatusSites, 
                 disallowedSite => disallowedSite.Equals(site, StringComparison.OrdinalIgnoreCase));
    }
}
