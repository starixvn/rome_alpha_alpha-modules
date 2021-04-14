namespace Alpha.DNAide.Data
{
    using DNAide.Utils;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using System;
    using System.Configuration;
    using System.Data.Common;

    public static class DbConnSettings
    {
        private static readonly ICacheService cacheManager;

        static DbConnSettings()
        {
            DbConnSettings.cacheManager = new CacheService(new MemoryCache(new MemoryCacheOptions()));
        }

        public static DbConnection GetDbConnection(string dbType)
        {
            DbConnection connectionString = null;
            ConnectionStringSettings connectionStringSetting = DbConnSettings.cacheManager.Get<ConnectionStringSettings>(string.Concat("connSetting_", dbType));
            if (connectionStringSetting == null)
            {
                ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[dbType];
                connectionStringSetting = new ConnectionStringSettings(item.Name, Encryption.GetDeCryptConnectionString(item.ConnectionString), item.ProviderName);
                DbConnSettings.cacheManager.Set<ConnectionStringSettings>(string.Concat("connSetting_", dbType), connectionStringSetting, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(120)));
            }
            DbProviderFactory factory = DbProviderFactories.GetFactory(connectionStringSetting.ProviderName);
            if (factory == null)
            {
                return connectionString;
            }
            connectionString = factory.CreateConnection();
            connectionString.ConnectionString = connectionStringSetting.ConnectionString;
            return connectionString;
        }
    }
}
