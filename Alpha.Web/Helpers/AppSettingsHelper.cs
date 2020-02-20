namespace Alpha.Web.Helpers
{
    using Alpha.Web.Configurations;
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class AppSettingsHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static bool GetUpdateCreditReplicateServer() => AppSettings.UpdateCreditBothDBCustAndDBAge;

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static int GetInsertProfilePictureSiteMode() => AppSettings.InsertProfilePictureSiteMode;

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static bool CheckSiteGroup(string site) => AppSettings.SiteGroup.Equals(site, StringComparison.OrdinalIgnoreCase);
    }
}
