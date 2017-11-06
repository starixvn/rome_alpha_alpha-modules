namespace Sunplus.Agent.Model.DL.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using Fanex.Data.DNAide;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.DL.Definitions;


    [Obsolete("Please rename to Sunplus.Agent.Model.DataAccessLibrary")]
    public class DAOBase
    {
        private const int DefaultTimeOut = 600;
        private const string NoSp = "-";

        #region ExecSp to DbResult

        public virtual DataResult GetResult(ObjectDb db)
        {
            string errCode = db.GetParameterValue("errcode").ToString();
            string errMsg = db.GetParameterValue("errmsg").ToString();
            return new DataResult(errCode, errMsg);
        }

        public virtual IList<DbResult> ExecSp(string name, IList<Hashtable> parameters, int timeOut, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(name, parameters[i], DefaultTimeOut));
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(IList<string> names, IList<Hashtable> parameters, int timeOut, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < names.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(names[i], parameters[i], DefaultTimeOut));
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(string name, IList<Hashtable> parameters, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(name, parameters[i], DefaultTimeOut));
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(IList<string> names, IList<Hashtable> parameters, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < names.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(names[i], parameters[i], DefaultTimeOut));
            }

            return results;
        }

        /// <summary>
        /// Base method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DbResult ExecSp(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = null;

            var dtbSetting = DbSettingProviderManager.Current.GetSetting(name);
            string sp = dtbSetting.SpName;
            string asType = dtbSetting.DbName;

            if (sp == NoSp)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                return new DbResult(new Hashtable(), ds);
            }

            bool useLiveDb = ConfigurationManager.AppSettings["UseLiveDb"] == "1";
            bool useLiveMainDb = ConfigurationManager.AppSettings["UseLiveMainDb"] == "1";

            if (useLiveDb && (
                (asType == DbTypes.MAIN && useLiveMainDb) ||
                asType == DbTypes.ARCHIVE ||
                asType == DbTypes.ARCHIVE5 ||
                asType == DbTypes.ARCHIVE52 ||
                asType == DbTypes.ARCHIVE53 ||
                asType == DbTypes.ARCHIVE6 ||
                asType == DbTypes.WAREHOUSE))
            {
                LiveDataService service = new LiveDataService();
                List<string> parameterNames = new List<string>();
                List<object> parameterValues = new List<object>();

                if (parameters != null)
                {
                    foreach (object key in parameters.Keys)
                    {
                        parameterNames.Add(key as string);
                        parameterValues.Add(parameters[key]);
                    }
                }

                dtbResult = (DbResult)service.Invoke(name, parameterNames.ToArray(), parameterValues.ToArray(), asType);
            }
            else
            {
                dtbResult = DbHelper.ExecuteDbResult(name, parameters, timeOut);
            }

            return dtbResult;
        }

        public DbResult ExecSp(string name, Hashtable parameters)
        {
            return ExecSp(name, parameters, DefaultTimeOut);
        }

        #endregion

        #region ExecSp to DataTable
        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters)
        {
            return ExecSpTable(name, parameters, DefaultTimeOut);
        }

        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters, int timeOut)
        {
            return ExecSpTable(name, parameters, timeOut, true);
        }

        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <param name="isVerifyingReturnData"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters, int timeOut, bool isVerifyingReturnData)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null && dtbResult.ResultSet.Tables.Count > 0)
            {
                return dtbResult.ResultSet.Tables[0];
            }

            if (isVerifyingReturnData)
            {
                // Throw DataException and log the SP callingm
                Sunplus.Agent.Model.DL.AgentDataAccessException error = new Sunplus.Agent.Model.DL.AgentDataAccessException(name, parameters);
                ExceptionHandler.Handle(error);
                throw error;
            }

            return null;
        }

        #endregion

        #region ExecSp To DataSet
        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual DataSet ExecSpDataset(string name, Hashtable parameters)
        {
            return ExecSpDataset(name, parameters, DefaultTimeOut);
        }

        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DataSet ExecSpDataset(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null)
            {
                return dtbResult.ResultSet;
            }

            //// Throw DataException and log the SP calling
            Sunplus.Agent.Model.DL.AgentDataAccessException error = new Sunplus.Agent.Model.DL.AgentDataAccessException(name, parameters);
            ExceptionHandler.Handle(error);
            throw error;
        }

        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual DataSet ExecSp(string name, ref Hashtable parameters)
        {
            DataSet dataSet = null;
            DbResult dtbResult = ExecSp(name, parameters, DefaultTimeOut);

            // Check exist a DataTable in DataSet
            if (dtbResult != null)
            {
                Hashtable outputs = dtbResult.Outputs;

                foreach (string paramName in outputs.Keys)
                {
                    if (parameters.ContainsKey(paramName))
                    {
                        parameters[paramName] = outputs[paramName];
                    }
                    else
                    {
                        parameters.Add(paramName, outputs[paramName]);
                    }
                }

                dataSet = dtbResult.ResultSet;
            }

            return dataSet;
        }

        #endregion

        #region ExecSp To Hashtable

        /// <summary>
        /// Execute use store procedure Return hash table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual Hashtable ExecSpHashtable(string name, Hashtable parameters)
        {
            return ExecSpHashtable(name, parameters, DefaultTimeOut);
        }

        /// <summary>
        /// Execute use store procedure Return hash table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual Hashtable ExecSpHashtable(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null && dtbResult.Outputs != null && dtbResult.Outputs.Count > 0)
            {
                return dtbResult.Outputs;
            }

            //// Throw DataException and log the SP calling
            Sunplus.Agent.Model.DL.AgentDataAccessException error = new Sunplus.Agent.Model.DL.AgentDataAccessException(name, parameters);
            ExceptionHandler.Handle(error);
            throw error;
        }

        #endregion
    }
}
