namespace Alpha.IntegrationTests.Helpers
{
    using System;
    using System.IO;

    public static class FileHelper
    {
        public static string Load(string virtualPath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var absolutePath = Path.Combine(baseDirectory, virtualPath);

            if (File.Exists(absolutePath))
            {
                using (StreamReader file = new StreamReader(absolutePath))
                {
                    return file.ReadToEnd();
                }
            }

            return null;
        }
    }
}
