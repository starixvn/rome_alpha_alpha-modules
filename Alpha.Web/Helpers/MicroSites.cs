namespace Alpha.Web.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using Configurations;

    public static class MicroSites
    {
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string OldMainUrl(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MainUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MainAppPath, virtualPath);
        }
    }
}