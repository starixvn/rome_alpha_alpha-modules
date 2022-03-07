namespace Alpha.Web.Helpers
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using Alpha.Web.Models;
    using Configurations;
    using Sunplus.Agent.Model.Entities;

    public static class StaticResources
    {
        private static string staticResourcesVersion;

        [SuppressMessage("Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "Reviewed!")]
        private static string minText = ".min";

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed!")]
        public static void Init(string version)
        {
            if (IsDebugMode())
            {
                minText = string.Empty;
            }

            staticResourcesVersion = version;
        }

        public static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }

        private static bool IsDebugMode()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }

        private static IHtmlString RenderStyle(params string[] paths)
        {
            var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{ path }\" rel=\"stylesheet\" type=\"text/css\">"));

            return new HtmlString(styleHtml);
        }

        private static IHtmlString RenderStyleWithMedia(int maxDeviceWidth, params string[] paths)
        {
            var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{ path }\" rel=\"stylesheet\" type=\"text/css\" media=\"screen and (max-device-width: {maxDeviceWidth}px)\">"));

            return new HtmlString(styleHtml);
        }

        private static IHtmlString RenderScript(params string[] paths)
        {
            var scriptHtml = string.Join(string.Empty, paths.Select(path => $"<script src=\"{ path }\" type=\"text/javascript\"></script>"));

            return new HtmlString(scriptHtml);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static IHtmlString RenderScripts(params string[] paths)
        {
            var scriptHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.js?v={staticResourcesVersion}")));

            return RenderScript(scriptHtml);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static IHtmlString RenderStyles(params string[] paths)
        {
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyle(styleHtml);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static IHtmlString RenderStylesWithMedia(int maxDeviceWidth, params string[] paths)
        {
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyleWithMedia(maxDeviceWidth, styleHtml);
        }

        public static IHtmlString RenderStylesWithDarkMode(params string[] paths)
        {
            var darkModeName = GetDarkModeName();
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{darkModeName}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyle(styleHtml);
        }

        public static IHtmlString RenderStylesWithRoleAndDarkMode(params string[] paths)
        {
            var userSession = GetCurrentUserSession();
            var roleName = GetRoleName(userSession);
            var darkModeName = GetDarkModeName(userSession);
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{roleName}{darkModeName}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyle(styleHtml);
        }

        public static IHtmlString RenderStylesWithMediaDarkMode(int maxDeviceWidth, params string[] paths)
        {
            var darkModeName = GetDarkModeName();
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{darkModeName}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyleWithMedia(maxDeviceWidth, styleHtml);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetScripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                var assetAppPath = GetAssetAppPath();

                return RenderScript(paths.Select(path => $"{assetAppPath}{path}{minText}.js?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path)
            {
                var assetAppPath = GetAssetAppPath();
                var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderScript($"{assetAppPath}{path}{minText}.js{version}");
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetStyles
        {
            public static IHtmlString Render(params string[] paths)
            {
                var assetAppPath = GetAssetAppPath();

                return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path)
            {
                var assetAppPath = GetAssetAppPath();
                var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderStyle($"{assetAppPath}{path}{minText}.css{version}");
            }

            public static IHtmlString RenderWithDarkMode(params string[] paths)
            {
                var userSession = GetCurrentUserSession();
                var assetAppPath = GetAssetAppPath(userSession);
                var darkModeName = GetDarkModeName(userSession);

                return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderWithRoleAndDarkMode(params string[] paths)
            {
                var userSession = GetCurrentUserSession();
                var assetAppPath = GetAssetAppPath(userSession);
                var roleName = GetRoleName(userSession);
                var darkModeName = GetDarkModeName(userSession);

                return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{roleName}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommonWithDarkMode(string path)
            {
                var userSession = GetCurrentUserSession();
                var assetAppPath = GetAssetAppPath(userSession);
                var darkModeName = GetDarkModeName(userSession); var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderStyle($"{assetAppPath}{path}{darkModeName}{minText}.css{version}");
            }
        }

        private static string GetAssetAppPath(IUserSession userSession = null)
        {
            userSession = userSession ?? GetCurrentUserSession();
            var assetAppPath = userSession.AssetAppPath;

            return string.IsNullOrWhiteSpace(assetAppPath)
                ? AppSettings.AssetAppPath
                : assetAppPath;
        }

        private static string GetRoleName(IUserSession userSession = null)
        {
            userSession = userSession ?? GetCurrentUserSession();

            return Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLowerInvariant();
        }

        private static string GetDarkModeName(IUserSession userSession = null)
        {
            userSession = userSession ?? GetCurrentUserSession();

            return AppSettings.IsOfferDarkMode ? userSession.DarkModeName : string.Empty;
        }

        private static IUserSession GetCurrentUserSession() => UserSession.Current;
    }
}