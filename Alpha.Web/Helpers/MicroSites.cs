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

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string ReportsUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.ReportsAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string BetListsUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.BetListsAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MemberInfoUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MemberInfoAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MessageUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MessageAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string ExTotalBetsForecastUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.ExTotalBetsForecastAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string HowToUseUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);
        }
    }
}
