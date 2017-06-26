namespace Alpha.Announcements.Client.Configurations
{
    using System.Configuration;

    public class AppSettings
    {
        public static string ApiUrl = ConfigurationManager.AppSettings["Announcement:ApiUrl"];

        public static string SecretKey = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Announcement:SecretKey"])
                ? "ZDlhMWFmMDgtZGQ0ZC00NWY0LTg1ODItNzVkNTdhOTNjOTEx"
                : ConfigurationManager.AppSettings["Announcement:SecretKey"];
    }
}