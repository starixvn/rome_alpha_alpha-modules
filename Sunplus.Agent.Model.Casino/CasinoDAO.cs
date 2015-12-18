namespace Sunplus.Agent.Model.DL.Casino
{
    using System;
    using System.Collections;
    using Sunplus.Agent.Model.DL.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.Casino")]
    public class CasinoDAO : DAOBase
    {
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
    }
}