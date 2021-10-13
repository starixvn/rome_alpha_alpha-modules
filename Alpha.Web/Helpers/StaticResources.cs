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
            var darkModeName = userSession.DarkModeName;
            var roleName = Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLowerInvariant();

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
                return RenderScript(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{minText}.js?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path)
            {
                var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderScript($"{AppSettings.AssetAppPath}{path}{minText}.js{version}");
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetStyles
        {
            public static IHtmlString Render(params string[] paths)
            {
                return RenderStyle(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path)
            {
                var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderStyle($"{AppSettings.AssetAppPath}{path}{minText}.css{version}");
            }

            public static IHtmlString RenderWithDarkMode(params string[] paths)
            {
                var darkModeName = GetDarkModeName();

                return RenderStyle(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderWithRoleAndDarkMode(params string[] paths)
            {
                var userSession = GetCurrentUserSession();
                var darkModeName = userSession.DarkModeName;
                var roleName = Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLowerInvariant();

                return RenderStyle(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{roleName}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommonWithDarkMode(string path)
            {
                var darkModeName = GetDarkModeName();
                var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

                return RenderStyle($"{AppSettings.AssetAppPath}{path}{darkModeName}{minText}.css{version}");
            }
        }

        private static string GetDarkModeName() => GetCurrentUserSession().DarkModeName;

        private static IUserSession GetCurrentUserSession() => UserSession.Current;
    }
}