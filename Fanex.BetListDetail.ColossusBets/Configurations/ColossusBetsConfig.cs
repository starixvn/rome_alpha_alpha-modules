namespace Fanex.BetListDetail.ColossusBets.Configurations
{
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Web;
    using System.Web.Script.Serialization;

    public static class AppSettings
    {
        static AppSettings()
        {
            LoadConfig();
        }

        public static ColossusBets ColossusBetsConfig { get; internal set; }

        public static string FilePath => ConfigurationManager.AppSettings["ColossusBets:ConfigFilePath"] ?? "~/App_Data/ColossusBetsConfig.json";

        internal static void LoadConfig(string content = null)
        {
            if (HttpContext.Current != null
               && HttpContext.Current.Server != null)
            {
                var configFilePath = HttpContext.Current.Server.MapPath(FilePath);
                if (File.Exists(configFilePath))
                {
                    var text = File.ReadAllText(configFilePath);
                    var serializer = new JavaScriptSerializer();
                    ColossusBetsConfig = serializer.Deserialize<ColossusBets>(text);
                }
                else
                {
                    throw new FileNotFoundException("Cannot find security code json file");
                }
            }
        }
    }
}
