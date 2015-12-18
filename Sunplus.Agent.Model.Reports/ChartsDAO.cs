namespace Sunplus.Agent.Model.DL.Charts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.DL.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.Reports")]
    public class ChartsDAO : DAOBase
    {
        #region Charting Winloss Analysis
        /// <summary>
        /// Get winloss analysis for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="balanceUpDay"></param>
        /// <returns></returns>
        public DataTable Age_Chart_WinlossAnalysis_Ag(
            int custId, 
            DateTime fromDate, 
            DateTime toDate, 
            ICollection<KeyValuePair<string, bool>> filterParams, 
            DateTime balanceUpDay)
        {
            var parameters = new Hashtable
            {
                { "agentid", custId },
                { "fromdate", fromDate },
                { "todate", toDate },
                { "balanceupdate", balanceUpDay }
            };

            foreach (var element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            return ExecSpTable("Age_Chart_WinlossAnalysis_Ag", parameters);
        }

        /// <summary>
        /// Get winloss analysis for Master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="balanceUpDay"></param>
        /// <returns></returns>
        public DataTable Age_Chart_WinlossAnalysis_MAg(
            int custId, 
            DateTime fromDate, 
            DateTime toDate, 
            ICollection<KeyValuePair<string, bool>> filterParams, 
            DateTime balanceUpDay)
        {
            var parameters = new Hashtable
            {
                { "agentid", custId },
                { "fromdate", fromDate },
                { "todate", toDate },
                { "balanceupdate", balanceUpDay }
            };

            foreach (var element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            return ExecSpTable("Age_Chart_WinlossAnalysis_MAg", parameters);
        }

        /// <summary>
        /// Get winloss analysis for Super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="balanceUpDay"></param>
        /// <returns></returns>
        public DataTable Age_Chart_WinlossAnalysis_SAg(
            int custId, 
            DateTime fromDate, 
            DateTime toDate, 
            ICollection<KeyValuePair<string, bool>> filterParams, 
            DateTime balanceUpDay)
        {
            var parameters = new Hashtable
            {
                { "agentid", custId },
                { "fromdate", fromDate },
                { "todate", toDate },
                { "balanceupdate", balanceUpDay }
            };

            foreach (var element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            return ExecSpTable("Age_Chart_WinlossAnalysis_SAg", parameters);
        }
        #endregion
    }
}
