namespace Alpha.SecurityCode.Configurations
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Web;

    public static class AppSettings
    {
        static AppSettings()
        {
            LoadSecurityCodeConfig();
        }

        public static SecurityCodeConfig SecurityCodeConfig { get; internal set; }

        public static string SecurityCodeConfigFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["SecurityCode:ConfigFilePath"] ?? "~/App_Data/securitycode.json";
            }
        }

        private static void LoadSecurityCodeConfig()
        {
            if (HttpContext.Current != null
                && HttpContext.Current.Server != null)
            {
                var configFilePath = HttpContext.Current.Server.MapPath(SecurityCodeConfigFilePath);
                if (File.Exists(configFilePath))
                {
                    using (Stream stream = File.OpenRead(configFilePath))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(SecurityCodeConfig));
                        SecurityCodeConfig = (SecurityCodeConfig)serializer.ReadObject(stream);
                    }
                }
                else
                {
                    throw new FileNotFoundException("Cannot find security code json file");
                }
            }
        }
    }
}