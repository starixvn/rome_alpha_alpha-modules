namespace Alpha.DNAide.Data
{
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public static class DbCmdFactory
    {
        private static readonly ICacheService cacheManager;

        static DbCmdFactory()
        {
            DbCmdFactory.cacheManager = new CacheService(new MemoryCache(new MemoryCacheOptions()));
        }

        public static DbCommand GetDbCmd(string cmdText, string dbType)
        {
            DbCommand dbCommand = DbCmdFactory.cacheManager.Get<DbCommand>(string.Concat(dbType, ".", cmdText));
            ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[dbType];
            DbProviderFactory factory = DbProviderFactories.GetFactory(item.ProviderName);
            DbCommand dbCommand1 = null;
            if (dbCommand != null)
            {
                return DbCmdFactory.CloneDbCmd(dbCommand, factory);
            }
            dbCommand = factory.CreateCommand();
            dbCommand.CommandText = cmdText;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Connection = factory.CreateConnection();
            try
            {
                if (item.ProviderName.Contains("SqlClient"))
                {
                    dbCommand.Connection.Open();
                    SqlCommandBuilder.DeriveParameters((SqlCommand)dbCommand);
                }
                DbCmdFactory.AddCmd2Cache(dbType, dbCommand);
                dbCommand1 = DbCmdFactory.CloneDbCmd(dbCommand, factory);
            }
            catch
            {
                if (dbCommand.Connection.State == ConnectionState.Open)
                {
                    dbCommand.Connection.Close();
                }
                dbCommand.Connection = null;
            }
            return dbCommand1;
        }

        public static void AddCmd2Cache(string dbType, DbCommand cmd)
        {
            DbCmdFactory.cacheManager.Set<DbCommand>(string.Concat(dbType, ".", cmd.CommandText.ToString()), cmd, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(120)));
        }

        private static DbCommand CloneDbCmd(DbCommand cmdSrc, DbProviderFactory dpFactory)
        {
            DbCommand commandText = dpFactory.CreateCommand();
            try
            {
                commandText.CommandText = cmdSrc.CommandText;
                commandText.CommandType = cmdSrc.CommandType;
                foreach (DbParameter parameter in cmdSrc.Parameters)
                {
                    DbParameter dbType = dpFactory.CreateParameter();
                    dbType.DbType = parameter.DbType;
                    dbType.Direction = parameter.Direction;
                    dbType.ParameterName = parameter.ParameterName.ToLower();
                    dbType.Size = parameter.Size;
                    dbType.SourceColumn = parameter.SourceColumn;
                    dbType.SourceVersion = parameter.SourceVersion;
                    dbType.SourceColumnNullMapping = parameter.SourceColumnNullMapping;
                    commandText.Parameters.Add(dbType);
                }
            }
            catch
            {
                commandText.Dispose();
                throw;
            }
            return commandText;
        }

        public static SqlCommand GetSqlCmd(string cmdText, string dbType)
        {
            SqlCommand sqlCommand = (SqlCommand)DbCmdFactory.cacheManager.Get<DbCommand>(string.Concat(dbType, ".", cmdText));
            if (sqlCommand != null)
            {
                return sqlCommand.Clone();
            }
            sqlCommand = new SqlCommand()
            {
                CommandText = cmdText,
                CommandType = CommandType.StoredProcedure,
                Connection = (SqlConnection)DbConnSettings.GetDbConnection(dbType)
            };
            try
            {
                try
                {
                    sqlCommand.Connection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    DbCmdFactory.AddCmd2Cache(dbType, sqlCommand);
                }
                catch
                {
                    throw;
                }
            }
            finally
            {
                if (sqlCommand.Connection != null && sqlCommand.Connection.State == ConnectionState.Open)
                {
                    sqlCommand.Connection.Close();
                }
                sqlCommand.Connection = null;
            }
            return sqlCommand.Clone();
        }
    }
}
