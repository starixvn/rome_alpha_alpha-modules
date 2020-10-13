namespace Alpha.CopyAccount.App_Start
{
    using Alpha.CopyAccount.Authentications;
    using Alpha.CopyAccount.Services;
    using RestSharp;
    using SimpleInjector;
    using System;
    using System.Configuration;

    public static class Configuration
    {
        public static void RegisterCopyAccountServices(this Container container, string apiUri, string apiKey)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            container.RegisterPerWebRequest<IRestClient>(() => new RestClient
            {
                BaseUrl = new Uri(ConfigurationManager.AppSettings[apiUri]),
                Authenticator = new HmacAuthenticator(ConfigurationManager.AppSettings[apiKey])
            });

            container.RegisterPerWebRequest<ICopyAccountService, CopyAccountService>();
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}