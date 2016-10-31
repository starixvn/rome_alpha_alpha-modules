namespace Sunplus.Agent.Model.DL.Casino
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.DNAide;

    [Obsolete("Should rename to Sunplus.Agent.Model.Casino")]
    public class CasinoDAO
    {
        private const int DefaultTimeOut = 600;
        private const string NoSp = "-";

        public void WriteLog_UserLog_CasinoAPI(
            byte systemId,
            int userId,
            int custId,
            int actionId,
            string funcName,
            string trackingValue,
            string returnValue,
            int returnCode)
        {
            var parameters = new Hashtable
            {
                { "systemid", systemId },
                { "userid", userId },
                { "custid", custId },
                { "actionid", actionId },
                { "funcname", funcName },
                { "trackingvalue", trackingValue },
                { "returnvalue", returnValue },
                { "returncode", returnCode }
            };

            ExecSp("WriteLog_UserLog_CasinoAPI", parameters);
        }

        private DbResult ExecSp(string name, Hashtable parameters, int timeOut = DefaultTimeOut)
        {
            DbResult dtbResult = null;

            var dtbSetting = DbSettingProviderManager.CurrentDbSettingProvider.GetSetting(name);
            string sp = dtbSetting.SpName;

            if (sp == NoSp)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                return new DbResult(new Hashtable(), ds);
            }

            dtbResult = DbHelper.ExecuteDbResult(name, parameters, timeOut);

            return dtbResult;
        }
    }
}