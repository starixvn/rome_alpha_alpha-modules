namespace Sunplus.Agent.Model.BL.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Caching;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Model.DL.Charts;

    [Obsolete("Should rename to Sunplus.Agent.Model.Reports")]
    public class ChartsBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        private const string ChartWinLossAnalysisKey = "SUNPLUS.AGENT.MODEL.BL.CHARTS.WINLOSSANALYSIS";

        public ChartsBO(string netSessionId)
            : base(netSessionId)
        {
        }

        public DataTable GetChartWinlossAnalysis(int custId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
            var permString = string.Empty;

            filterParams.ToList().ForEach(o =>
            {
                permString += o.Value;
            });

            var cacheKey = ChartWinLossAnalysisKey + custId.ToString() + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd") + permString;
            var oldData = cache.Get<DataTable>(cacheKey);

            if (oldData == null)
            {
                ChartsDAO dao = new ChartsDAO();
                DataTable dt = null;
                if (CurrentSession.IsAgent)
                {
                    dt = dao.Age_Chart_WinlossAnalysis_Ag(custId, fromDate, toDate, filterParams, ReportsBO.BalanceUpDate);
                }
                else if (CurrentSession.IsMaster)
                {
                    dt = dao.Age_Chart_WinlossAnalysis_MAg(custId, fromDate, toDate, filterParams, ReportsBO.BalanceUpDate);
                }
                else
                {
                    // CurrentSession.IsSuper
                    dt = dao.Age_Chart_WinlossAnalysis_SAg(custId, fromDate, toDate, filterParams, ReportsBO.BalanceUpDate);
                }

                // Store in cache
                if (dt.Rows.Count > 1)
                {
                    cache.Set<DataTable>(cacheKey, dt, TimeSpan.FromMinutes(3));
                }

                return dt;
            }

            return oldData as DataTable;
        }
    }
}