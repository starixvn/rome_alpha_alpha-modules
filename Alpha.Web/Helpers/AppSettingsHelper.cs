namespace Alpha.Web.Helpers
{
    using Alpha.Web.Configurations;
    using System.Diagnostics.CodeAnalysis;

    public static class AppSettingsHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static bool GetUpdateCreditReplicateServer() => AppSettings.UpdateCreditBothDBCustAndDBAge;
    }
}
