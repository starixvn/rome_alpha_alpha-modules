namespace Alpha.Web.Helpers
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common.Helper;

    public static class AppPathHelper
    {
        public static string GetAppPath(string appName, string virtualPath)
        {
            return GetAppPath(appName, virtualPath, null, null);
        }

        public static string GetAppPath(
            string appName,
            string virtualPath,
            Func<HttpContext> getCurrentContext,
            Func<string> getAppDomainVirtualPath)
        {
            Check.Argument.IsNotEmpty(appName, nameof(appName));
            Check.Argument.IsNotEmpty(virtualPath, nameof(virtualPath));

            getCurrentContext = getCurrentContext ?? (() => { return HttpContext.Current; });
            getAppDomainVirtualPath = getAppDomainVirtualPath ?? (() => HttpRuntime.AppDomainAppVirtualPath);

            var context = getCurrentContext();

            if (context == null || context.CurrentHandler == null)
            {
                return string.Empty;
            }

            var appPath = context.Response.ApplyAppPathModifier(virtualPath);
            var appDomainPath = getAppDomainVirtualPath();
            if (!appDomainPath.Equals("/", StringComparison.OrdinalIgnoreCase))
            {
                appPath = Regex.Replace(appPath, appDomainPath, appName, RegexOptions.IgnoreCase);
            }
            else
            {
                appPath = $"{appName}{appPath}";
            }

            return appPath;
        }

        public static string GetRelativePath(string virtualPath, string absolutePath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath) || string.IsNullOrWhiteSpace(absolutePath))
            {
                return virtualPath;
            }

            if (absolutePath.StartsWith(virtualPath, StringComparison.OrdinalIgnoreCase))
            {
                return absolutePath.Substring(virtualPath.Length + 1);
            }

            return absolutePath;
        }
    }
}