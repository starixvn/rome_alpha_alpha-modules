namespace Alpha.IntegrationTests.Fixtures
{
    using System;
    using System.IO;
    using MvcIntegrationTestFramework.Hosting;

    public abstract class AppHostFixture : IDisposable
    {
        private readonly string projectPath;

        protected AppHostFixture()
        {
            var virtualPath = SetWebsiteDirectory();

            projectPath = GetProjectPath(virtualPath);
            CopyConfigFile(projectPath);

            AppHost.LoadAllBinaries = true;
            AppHost = AppHost.Simulate(virtualPath);
        }

        public AppHost AppHost { get; }

        public void Dispose()
        {
            AppHost.Dispose();
            File.Copy("Web.Original.config", Path.Combine(projectPath, "Web.config"), true);
        }

        public abstract string SetWebsiteDirectory();

        private static string GetProjectPath(string virtualPath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (Path.GetPathRoot(baseDirectory) != baseDirectory)
            {
                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\", StringComparison.Ordinal));
                var mvcPath = Path.Combine(baseDirectory, virtualPath);

                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }
            }

            return null;
        }

        private void CopyConfigFile(string projectPath)
        {
            if (!string.IsNullOrEmpty(projectPath))
            {
                File.Copy("Web.Integration.config", Path.Combine(projectPath, "Web.config"), true);
            }
        }
    }
}
