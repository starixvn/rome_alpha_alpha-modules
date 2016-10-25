namespace Alpha.Web.Configurations
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string OldMainAppPath => ConfigurationManager.AppSettings["OldMainAppPath"];
    }
}