namespace Sunplus.Agent.Model.DL.Casino
{
    using System;
    using System.Collections;
    using Fanex.Data;

    [Obsolete("Should rename to Sunplus.Agent.Model.Casino")]
    public class CasinoDAO
    {
        private const int DefaultTimeOut = 600;

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

            using (var db = new ObjectDb("WriteLog_UserLog_CasinoAPI", DefaultTimeOut))
            {
                db.ExecuteNonQuery(parameters);
            }
        }
    }
}