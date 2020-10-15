namespace Alpha.DNAide.Data
{
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using Fanex.Data.DNAide;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    public static class DbHelper
    {
        private static DbResult _execute(string spName, Hashtable parameters, int timeout, byte cmdType)
        {
            IDbSetting setting = DbSettingProviderManager.Current.GetSetting(spName);
            string str = setting.SpName;
            string dbName = setting.DbName;
            string providerName = ConfigurationManager.ConnectionStrings[dbName].ProviderName;
            if (!string.IsNullOrEmpty(providerName) && !providerName.Contains(".SqlClient"))
            {
                return null;
            }
            return DbHelper.execSql(str, parameters, dbName, timeout, (int)cmdType);
        }

        private static DbResult execSql(string spName, Hashtable parameters, string dbType, int connectionTimeout, int cmdType)
        {
            SqlConnection dbConnection = null;
            DbResult dbResult = null;
            SqlCommand sqlCmd = null;
            SqlDataAdapter sqlDataAdapter = null;
            try
            {
                try
                {
                    sqlCmd = DbCmdFactory.GetSqlCmd(spName, dbType);
                    if (connectionTimeout > 0)
                    {
                        sqlCmd.CommandTimeout = connectionTimeout;
                    }
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in sqlCmd.Parameters)
                        {
                            if (parameter.ParameterName.Equals("@RETURN_VALUE"))
                            {
                                continue;
                            }
                            parameter.Value = parameters[parameter.ParameterName.Replace("@", "").ToLowerInvariant()];
                        }
                    }
                    dbConnection = (SqlConnection)DbConnSettings.GetDbConnection(dbType);
                    DataSet dataSet = null;
                    if (cmdType == 0)
                    {
                        sqlCmd.Connection = dbConnection;
                        dbConnection.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                    else if (cmdType == 1)
                    {
                        sqlDataAdapter = new SqlDataAdapter()
                        {
                            SelectCommand = sqlCmd
                        };
                        sqlDataAdapter.SelectCommand.Connection = dbConnection;
                        dbConnection.Open();
                        dataSet = new DataSet()
                        {
                            Locale = CultureInfo.CurrentCulture
                        };
                        sqlDataAdapter.Fill(dataSet);
                    }
                    Hashtable hashtables = new Hashtable();
                    foreach (SqlParameter sqlParameter in sqlCmd.Parameters)
                    {
                        if (sqlParameter.ParameterName.Equals("@RETURN_VALUE") || sqlParameter.Direction == ParameterDirection.Input)
                        {
                            continue;
                        }
                        hashtables.Add(sqlParameter.ParameterName.Replace("@", "").ToLowerInvariant(), sqlParameter.Value);
                    }
                    dbResult = new DbResult(hashtables, dataSet);
                }
                catch
                {
                    throw;
                }
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
                if (sqlDataAdapter != null)
                {
                    sqlDataAdapter.Dispose();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
            return dbResult;
        }

        public static DbResult ExecuteCmd(string spName, Hashtable parameters, int timeout)
        {
            return DbHelper._execute(spName, parameters, timeout, 0);
        }

        public static DbResult ExecuteCmd(string spName, Hashtable parameters)
        {
            return DbHelper._execute(spName, parameters, 0, 0);
        }

        public static DbResult Execute(string spName, Hashtable parameters, int timeout)
        {
            return DbHelper._execute(spName, parameters, timeout, 1);
        }

        public static DbResult Execute(string spName, Hashtable parameters)
        {
            return DbHelper._execute(spName, parameters, 0, 1);
        }
    }
}
