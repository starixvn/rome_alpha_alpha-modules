namespace Sunplus.Agent.Model.BL.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Fanex.Caching;
    using Fanex.Common;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.Reports;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.BetListMonitoring;

    [Obsolete("Should rename to Sunplus.Agent.Model.Reports")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    public class ReportsBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        #region BalanceUpDate and MoveBetsDate

        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";

        public static DataTable GetBalanceUpDay()
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

            object oldData = cache.Get<DataTable>(BalanceUpDayKey);
            if (oldData == null)
            {
                ReportsDAO dao = new ReportsDAO();
                DataTable dt = dao.Age_SelBalanceUpDay();

                // Store in cache
                cache.Set<DataTable>(BalanceUpDayKey, dt, DateTime.Now.AddMinutes(5));

                return dt;
            }

            return oldData as DataTable;
        }

        public static DateTime BalanceUpDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().Rows[0]["BalanceUpDay"]);
            }
        }

        public static DateTime MoveBetsDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().Rows[0]["MoveBetsDay"]);
            }
        }

        #endregion BalanceUpDate and MoveBetsDate

        #region Constant

        /// <summary>
        /// Use this constant do config use bettrans usp on winloss report or not
        /// </summary>
        private const bool IgnoredBetTrans = true;

        #endregion Constant

        #region Varialbe

        private static readonly ReportsDAO ReportDataAccess = new ReportsDAO();

        #endregion Varialbe

        #region Constructor

        public ReportsBO(string netSessionId)
            : base(netSessionId)
        {
        }

        #endregion Constructor

        #region Check role

        /// <summary>
        /// Prevent agent user the role of super and master.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private int CheckRoleIdConstrain(int roleId)
        {
            return roleId > CurrentSession.RoleId ? CurrentSession.RoleId : roleId;
        }

        #endregion Check role

        #region MatchResultEntered

        public DataTable GetMatchResultEntered(out DateTime today, out DateTime yesterday)
        {
            DataTable reportData = ReportDataAccess.Age_Rpt_MatchSelResultEntered();

            int numRow = reportData.Rows.Count;
            DataRow dtrYesterday = numRow > 0 ? reportData.Rows[0] : null;
            DataRow dtrToday = numRow > 1 ? reportData.Rows[1] : null;
            yesterday = numRow > 0 ? (DateTime)dtrYesterday["EventDate"] : DateTime.Today;
            today = numRow > 1 ? (DateTime)dtrToday["EventDate"] : DateTime.Today;

            DataTable result = new DataTable();
            result.Columns.Add("EventDate", typeof(string));
            result.Columns.Add("SportIndex", typeof(int));
            result.Columns.Add("SportType", typeof(string));
            result.Columns.Add("YesterdayEntered", typeof(int));
            result.Columns.Add("YesterdayNonEntered", typeof(int));
            if (numRow > 1)
            {
                result.Columns.Add("TodayEntered", typeof(int));
                result.Columns.Add("TodayNonEntered", typeof(int));
            }

            if (numRow > 0)
            {
                int totalEntered = 0, yesterdayEntered = 0;
                for (int i = 1, columnCount = reportData.Columns.Count - 1; i < columnCount; i += 2)
                {
                    DataRow dr = result.NewRow();
                    totalEntered = 0;
                    yesterdayEntered = 0;

                    dr["SportIndex"] = i;
                    dr["SportType"] = string.Empty;
                    dr["YesterdayEntered"] = dtrYesterday[i];
                    dr["YesterdayNonEntered"] = dtrYesterday[i + 1];
                    yesterdayEntered = Convert.ToInt32(dtrYesterday[i]) + Convert.ToInt32(dtrYesterday[i + 1]);

                    if (numRow > 1)
                    {
                        dr["TodayEntered"] = dtrToday[i];
                        dr["TodayNonEntered"] = dtrToday[i + 1];
                        totalEntered = Convert.ToInt32(dtrToday[i]) + Convert.ToInt32(dtrToday[i + 1]);
                    }

                    // Only add the non-empty rows
                    if (totalEntered > 0 || yesterdayEntered > 0)
                    {
                        result.Rows.Add(dr);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Return the table with 5 columns SportType, YesterDayEnter, YesterDayNonEnter, TodayEnter, TodayNonEnter
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public DataTable GetMatchResultEntered(out DateTime today)
        {
            var yesterday = DateTime.MinValue;
            today = DateTime.MinValue;

            var reportData = ReportDataAccess.Age_Rpt_MatchSelResultEntered();
            var matchEnteredData = new Dictionary<int, DataRow>();

            // Convert to table
            var result = new DataTable();
            result.Columns.Add("SportType", typeof(int));
            result.Columns.Add("SportName", typeof(string));
            result.Columns.Add("YesterdayEntered", typeof(int));
            result.Columns.Add("YesterdayNonEntered", typeof(int));
            result.Columns.Add("TodayEntered", typeof(int));
            result.Columns.Add("TodayNonEntered", typeof(int));

            foreach (DataRow dr in reportData.Rows)
            {
                var isToday = (int)dr["IsToday"] == 1;
                var eventDate = (DateTime)dr["EventDate"];
                var entered = (int)dr["Entered"];
                var nonEntered = (int)dr["NonEntered"];
                var sportType = Convert.ToInt32(dr["SportType"].ToString());

                yesterday = isToday ? eventDate.AddDays(-1) : eventDate;
                today = isToday ? eventDate : eventDate.AddDays(1);

                var matchEntered = matchEnteredData.ContainsKey(sportType)
                                ? matchEnteredData[sportType]
                                : null;

                if (matchEntered == null)
                {
                    matchEntered = result.NewRow();
                    matchEntered["SportType"] = sportType;
                    matchEntered["SportName"] = Cache.CacheRef.GetSportName(sportType);
                    matchEntered["TodayEntered"] = 0;
                    matchEntered["TodayNonEntered"] = 0;
                    matchEntered["YesterdayEntered"] = 0;
                    matchEntered["YesterdayNonEntered"] = 0;

                    matchEnteredData.Add(sportType, matchEntered);
                }

                if (isToday)
                {
                    matchEntered["TodayEntered"] = entered;
                    matchEntered["TodayNonEntered"] = nonEntered;
                }
                else
                {
                    matchEntered["YesterdayEntered"] = entered;
                    matchEntered["YesterdayNonEntered"] = nonEntered;
                }
            }

            if (today == DateTime.MinValue)
            {
                today = yesterday == DateTime.MinValue ? DateTime.Today : yesterday.AddDays(1);
            }

            foreach (var key in matchEnteredData.Keys)
            {
                result.Rows.Add(matchEnteredData[key]);
            }

            return result;
        }

        #endregion MatchResultEntered

        #region GetWinloss

        /// <summary>
        /// Get winloss for super agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossSuper(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_SAg(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_SAg_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss for master agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossMaster(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_MAg(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_MAg_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss for agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAgent(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_Ag(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = ReportDataAccess.Age_Rpt_WinLoss_Ag_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }

        #endregion GetWinloss

        #region Get Winloss Analysis

        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisSuper(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = ReportDataAccess.Age_Rpt_WinlossAnalysis_SAg(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisMaster(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = ReportDataAccess.Age_Rpt_WinlossAnalysis_MAg(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisAgent(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = ReportDataAccess.Age_Rpt_WinlossAnalysis_Ag(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        #endregion Get Winloss Analysis

        #region Consolidated Reports

        public DataTable GetConsolidate(int custId, DateTime fromDate, DateTime toDate, int currency, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable consolidatedReport = null;
            consolidatedReport = ReportDataAccess.Age_Rpt_ConsolidatedGroup(fromDate, toDate, custId, currency, reportType, filterParams, BalanceUpDate);
            return consolidatedReport;
        }

        public DataTable GetConsolidateCurrency(int custId)
        {
            DataTable consolidatedCurrency = null;
            consolidatedCurrency = ReportDataAccess.Age_Rpt_ConsolidatedGroupSelCurrency(custId);
            return consolidatedCurrency;
        }

        #endregion Consolidated Reports

        #region GetWinlossDetail

        /// <summary>
        /// Get winloss detail for super agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailSuper(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossDetail_SAg(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
            }
            else
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossDetail_SAg_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss detail for master agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="oldView"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailMaster(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, bool oldView, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (oldView)
            {
                if (this.CheckConstraintMoveBetsDay(fromDate))
                {
                    winloss = ReportDataAccess.Age_Rpt_WinLossDetail_MAg(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
                }
                else
                {
                    winloss = ReportDataAccess.Age_Rpt_WinLossDetail_MAg_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
                }
            }
            else
            {
                if (this.CheckConstraintMoveBetsDay(fromDate))
                {
                    winloss = ReportDataAccess.Age_Rpt_WinLossDetail_MAg_New(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
                }
                else
                {
                    winloss = ReportDataAccess.Age_Rpt_WinLossDetail_MAg_bettrans_New(fromDate, toDate, customerId, filterParams, pagingParams);
                }
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss detail for agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailAgent(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossDetail_Ag(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
            }
            else
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossDetail_Ag_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
            }

            return winloss;
        }

        #endregion GetWinlossDetail

        #region Winloss Helper

        /// <summary>
        /// Check constraint between Lower Bound Date and Move Bet Date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private bool CheckConstraintMoveBetsDay(DateTime fromDate)
        {
            // isIgnoredBettrans=true : ignored constraint
            return DateTime.Compare(fromDate, MoveBetsDate) <= 0 || IgnoredBetTrans;
        }

        #endregion Winloss Helper

        #region Outstanding

        /// <summary>
        /// Get member outstanding
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetMemberOutstanding(int custId, int roleId)
        {
            DataTable outstanding = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (CurrentSession.IsAgent || roleId == 2)
            {
                outstanding = ReportDataAccess.Age_Rpt_Outstanding_Ag(custId);
            }
            else if (CurrentSession.IsMaster || roleId == 3)
            {
                outstanding = ReportDataAccess.Age_Rpt_Outstanding_MAg(custId);
            }
            else if (CurrentSession.IsSuper)
            {
                outstanding = ReportDataAccess.Age_Rpt_Outstanding_SAg(custId);
            }
            else
            {
                outstanding = new DataTable();
            }

            return outstanding;
        }

        #endregion Outstanding

        #region MatchWinloss

        /// <summary>
        /// Get match winloss detail
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetMatchWinloss(DateTime dateFrom, DateTime dateTo)
        {
            DataTable winloss = null;

            int customerID = CurrentSession.CustId;

            // execute
            if (CurrentSession.IsAgent)
            {
                winloss = ReportDataAccess.Age_Rpt_MatchWinLoss_Ag(customerID, dateFrom, dateTo);
            }
            else
            {
                winloss = new DataTable();
            }

            return winloss;
        }

        #endregion MatchWinloss

        #region MatchWinlossDetail

        /// <summary>
        /// Get Leagues by Sport
        /// </summary>
        /// <returns></returns>
        public DataTable GetLeagueBySport(DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DateTime moveBetsDate = MoveBetsDate;
            string language = CurrentSession.LangKey;
            return ReportDataAccess.Age_Rpt_MatchWinloss_SelLeagueBySport(CurrentSession.CustId, fromDate, toDate, filterParams, language, moveBetsDate);
        }

        /// <summary>
        /// Get match winloss detail
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetMatchWinlossDetail(DateTime dateFrom, DateTime dateTo, ICollection<KeyValuePair<string, bool>> filterParams, string league)
        {
            DataTable winlossDetail = null;

            int customerID = CurrentSession.CustId;
            DateTime moveBetsDate = MoveBetsDate;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                winlossDetail = ReportDataAccess.Age_Rpt_MatchWinLossDetail_Ag(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate);
            }
            else if (CurrentSession.IsMaster)
            {
                winlossDetail = ReportDataAccess.Age_Rpt_MatchWinLossDetail_MAg(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate);
            }
            else if (CurrentSession.IsSuper)
            {
                winlossDetail = ReportDataAccess.Age_Rpt_MatchWinLossDetail_SAg(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate);
            }
            else
            {
                winlossDetail = new DataTable();
            }

            return winlossDetail;
        }

        #endregion MatchWinlossDetail

        #region WinlossByBetType

        /// <summary>
        /// Get winloss by bettype
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetWinlossByBetType(DateTime dateFrom, DateTime dateTo)
        {
            DataTable winloss = null;

            int customerID = CurrentSession.CustId;
            DateTime balanceupDate = BalanceUpDate;

            // execute
            if (CurrentSession.IsAgent)
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossByBettype_Ag(customerID, dateFrom, dateTo, balanceupDate);
            }
            else if (CurrentSession.IsMaster)
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossByBettype_MAg(customerID, dateFrom, dateTo, balanceupDate);
            }
            else if (CurrentSession.IsSuper)
            {
                winloss = ReportDataAccess.Age_Rpt_WinLossByBettype_SAg(customerID, dateFrom, dateTo, balanceupDate);
            }
            else
            {
                winloss = new DataTable();
            }

            return winloss;
        }

        #endregion WinlossByBetType

        #region CommisionByBetType

        /// <summary>
        /// Get winloss by bettype
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetCommisionByBetType(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);
            DateTime moveBetDay = MoveBetsDate;

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_Ag(custId, dateFrom, dateTo, moveBetDay);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_MAg(custId, dateFrom, dateTo, moveBetDay);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_SAg(custId, dateFrom, dateTo, moveBetDay);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }

        #endregion CommisionByBetType

        #region FinanceCommision

        /// <summary>
        /// Get finance commission
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetFinanceCommision(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = ReportDataAccess.Age_Rpt_CommByFinance_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = ReportDataAccess.Age_Rpt_CommByFinance_MAg(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = ReportDataAccess.Age_Rpt_CommByFinance_SAg(custId, dateFrom, dateTo);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }

        #endregion FinanceCommision

        #region FinanceCommision

        /// <summary>
        /// Get HR commission
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetHRCommision(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_HR_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_HR_MAg(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = ReportDataAccess.Age_Rpt_CommByBettype_HR_SAg(custId, dateFrom, dateTo);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }

        #endregion FinanceCommision

        #region DailyStatment

        /// <summary>
        /// Get daily statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        public DataTable GetDailyStatement(int custId, int roleId, DateTime dateFrom, DateTime dateTo, Hashtable pagingParams)
        {
            DataTable statement = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_Ag(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_MAg(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_SAg(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else
            {
                statement = new DataTable();
            }

            return statement;
        }

        /// <summary>
        /// Get daily statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetDailyStatement(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable statement = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_MAg(custId);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                statement = ReportDataAccess.Age_Statement_Rpt_SAg(custId);
            }
            else
            {
                statement = new DataTable();
            }

            return statement;
        }

        #endregion DailyStatment

        #region OutrightResult

        /// <summary>
        /// Get outright result
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetOutrightResult(DateTime dateFrom, DateTime dateTo)
        {
            return ReportDataAccess.Age_Result_Outright(dateFrom, dateTo);
        }

        #endregion OutrightResult

        #region SportListByDate

        /// <summary>
        /// Get sport list by date
        /// Show to sport dropdown list in report result
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetResultSportByDate(DateTime date)
        {
            return ReportDataAccess.Age_Result_SportSelByDate(date);
        }

        #endregion SportListByDate

        #region ResultReport

        /// <summary>
        /// Get result of match
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sportId"></param>
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public DataTable GetResultMatchByLeague(DateTime date, int sportId, int leagueId)
        {
            string language = CurrentSession.Language;
            return ReportDataAccess.Age_Result_MatchSelByLeague(date, CurrentSession.CustId, sportId, leagueId, language);
        }

        /// <summary>
        /// Get result of match in data set.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sportId"></param>
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public DataSet GetResultMatchByLeagueDS(DateTime date, int sportId, int leagueId)
        {
            string language = CurrentSession.Language;

            return ReportDataAccess.Age_Result_MatchSelByLeagueDS(date, CurrentSession.CustId, sportId, leagueId, language);
        }

        /// <summary>
        /// Get result for number game
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public DataTable GetResultNumberGameByLeague(DateTime date, int leagueId)
        {
            string language = CurrentSession.Language;
            return ReportDataAccess.Age_Result_MatchSelNumberGameByLeague(date, leagueId, language);
        }

        /// <summary>
        /// Get result of horse racing
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public DataTable GetResultHRRaces(DateTime date, int sportId, int leagueId)
        {
            return ReportDataAccess.Age_Result_RaceSelByMeeting(date, sportId, leagueId);
        }

        /// <summary>
        /// Get result of virtual horse racing
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public DataTable GetResultVirtualHRRaces(DateTime date, int sportId, int leagueId)
        {
            return ReportDataAccess.Age_Result_VirtualHRSelByMeeting(date, sportId, leagueId);
        }

        /// <summary>
        /// Get match result of number game
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public DataTable SelectMatchForViewNumberGameResult(int matchId)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultNumberGame(matchId);
        }

        /// <summary>
        /// Get match result by matchId and race number
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataTable SelectMatchForViewResult(int matchId, string raceno)
        {
            return ReportDataAccess.Age_BetList_SelMatchResult(matchId, raceno);
        }

        /// <summary>
        /// Get match result by matchId and race number in data set.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataSet SelectMatchForViewResultDS(int matchId, string raceno)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultDS(matchId, raceno);
        }

        public DataSet SelectMatchForViewResultTennisDS(int matchId, string raceno, int bettype, int betid)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultTennisDS(matchId, raceno, bettype, betid);
        }

        /// <summary>
        /// Get match detail.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataSet SelectMatchResultDetail(int matchId)
        {
            return ReportDataAccess.Age_Result_MatchSelResultDetail(matchId);
        }

        /// <summary>
        /// Get match result Sporting Solutions
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataTable SelectSetDetailForViewResult(int matchId, string resourceId)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultSS(matchId, resourceId);
        }

        #endregion ResultReport

        #region GetLeagueByDate

        /// <summary>
        /// Get all leagues by date
        /// Show to league dropdown list in report result
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sportId"></param>
        /// <returns></returns>
        public DataTable GetResultLeagueByDate(DateTime date, int sportId)
        {
            string language = CurrentSession.Language;

            // Racing
            if (sportId == 151)
            {
                if (!AppSettings.Disable151_31)
                {
                    return ReportDataAccess.Age_Result_LeagueSelByDate(date, sportId, language);
                }
                else
                {
                    return new DataTable();
                }
            }
            else
            {
                return ReportDataAccess.Age_Result_LeagueSelByDate(date, sportId, language);
            }
        }

        #endregion GetLeagueByDate

        #region For Last Bets Monitor

        public DataTable GetLastBetList(int sportTypeGroup, int betTypeGroup, long transId, int limitRowCount)
        {
            DataTable dtbResult = GetRunningTickets(sportTypeGroup, betTypeGroup, transId, limitRowCount);
            if (dtbResult == null || dtbResult.Rows.Count == 0)
            {
                return new DataTable();
            }

            // Load match from cache (key: matchid) and fill (matchcode, sporttype, leagueid, homeid, awayid, race) in dtResult
            dtbResult.Columns.Add("showtime", typeof(string));
            DataRowCollection rows = dtbResult.Rows;

            foreach (DataRow row in rows)
            {
                int betType = Convert.ToInt32(row["bettype"]);
                var runningMatchs = GetRunningMatch(false, betType);
                int matchId = 0;

                string bettypeVirtualSportsOutrightList = AppSettings.MonitoringBetListBettypeVirtualSportsOutrightList;
                if (!string.IsNullOrWhiteSpace(bettypeVirtualSportsOutrightList) &&
                Regex.IsMatch(string.Format(",{0},", bettypeVirtualSportsOutrightList), string.Format(",{0},", betType), RegexOptions.IgnoreCase))
                {
                    matchId = Converter.ToInt(row["race"]);
                }
                else
                {
                    matchId = Converter.ToInt(row["matchid"]);
                }

                IEnumerable<MatchDetailInformation> matchInfoByMatchId = null;
                if (runningMatchs != null && runningMatchs.Any())
                {
                    matchInfoByMatchId = from q in runningMatchs where q.MatchId == matchId select q;
                }

                // Min match id, in case someone hard code match id for other purpose
                if ((matchInfoByMatchId == null || !matchInfoByMatchId.Any()) && matchId > 999)
                {
                    runningMatchs = GetRunningMatch(true, betType);
                    if (runningMatchs != null && runningMatchs.Any())
                    {
                        matchInfoByMatchId = from q in runningMatchs where q.MatchId == matchId select q;
                    }
                }

                if (matchInfoByMatchId == null || !matchInfoByMatchId.Any())
                {
                    continue;
                }

                var matchDetailInfo = matchInfoByMatchId.First();

                row["matchcode"] = matchDetailInfo.MatchCode;
                row["sporttype"] = matchDetailInfo.SportType;
                row["leagueid"] = matchDetailInfo.LeagueId;
                row["homeid"] = matchDetailInfo.HomeId;
                row["awayid"] = matchDetailInfo.AwayId;
                row["race"] = matchDetailInfo.Race;
                row["showtime"] = matchDetailInfo.ShowTime;
            }

            return dtbResult;
        }

        private const string RunningMatchKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.RUNNINGMATCH.CACHE.KEY";
        private const string OutrightMatchKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.OUTRIGHTMATCH.CACHE.KEY";
        private const string VirtualSportMatchKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.VIRTUALSPORTS.CACHE.KEY";
        private static bool isGetRunningMatch = false;
        private static object getRunningMatchLocker = new object();

        /// <summary>
        /// matchid is key, value is DataRow. At one time, just one GetRunningMatch was called.
        /// </summary>
        /// <param name="refreshCache"></param>
        /// <returns></returns>
        private static IEnumerable<MatchDetailInformation> GetRunningMatch(bool refreshCache, int betType)
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
            bool isOutright = false;
            bool isVirtualSportsOutright = false;
            string bettypeOutrightList = AppSettings.MonitoringBetListBettypeOutrightList;
            string bettypeVirtualSportsOutrightList = AppSettings.MonitoringBetListBettypeVirtualSportsOutrightList;
            string cacheName = RunningMatchKey;

            if (!string.IsNullOrWhiteSpace(bettypeOutrightList) &&
                Regex.IsMatch(string.Format(",{0},", bettypeOutrightList), string.Format(",{0},", betType), RegexOptions.IgnoreCase))
            {
                isOutright = true;
                cacheName = OutrightMatchKey;
            }
            else if (!string.IsNullOrWhiteSpace(bettypeVirtualSportsOutrightList) &&
                Regex.IsMatch(string.Format(",{0},", bettypeVirtualSportsOutrightList), string.Format(",{0},", betType), RegexOptions.IgnoreCase))
            {
                isVirtualSportsOutright = true;
                cacheName = VirtualSportMatchKey;
            }

            var oldData = cache.Get<IEnumerable<MatchDetailInformation>>(cacheName);

            if ((null == oldData || refreshCache) && !isGetRunningMatch)
            {
                lock (getRunningMatchLocker)
                {
                    if (!isGetRunningMatch)
                    {
                        isGetRunningMatch = true;

                        DataTable dtbMatch = isOutright || isVirtualSportsOutright ? ReportDataAccess.Age_Monitor_OutrightSel(isVirtualSportsOutright) : ReportDataAccess.Age_Monitor_MatchSel(0);
                        IEnumerable<MatchDetailInformation> mappingResult = MappingMonitoringBetListFromDatatableToObject(dtbMatch);
                        cache.Set<IEnumerable<MatchDetailInformation>>(cacheName, mappingResult, DateTime.Now.AddHours(1));
                        isGetRunningMatch = false;

                        return mappingResult;
                    }
                }
            }

            return oldData;
        }

        private DataTable GetRunningTickets(int sportTypeGroup, int betTypeGroup, long transId, int limitRowCount)
        {
            int custId = CurrentSession.CustId;
            int roleId = CurrentSession.RoleId;

            DataTable dtbResult = new DataTable();
            if (roleId == (int)UserTypes.Agent)
            {
                switch (betTypeGroup)
                {
                    case (int)BetType.None: // All bet types
                        dtbResult = ReportDataAccess.Age_Monitor_BetlistSel_Ag_AllBettypes(custId, transId, limitRowCount);
                        break;

                    case (int)BetType.Handicap:
                        dtbResult = ReportDataAccess.Age_Monitor_BetlistSel_Ag_HDP(custId, transId, limitRowCount);
                        break;

                    case (int)BetType.Over_Under:
                        dtbResult = ReportDataAccess.Age_Monitor_BetlistSel_Ag_OU(custId, transId, limitRowCount);
                        break;

                    case (int)BetType.Others: // Other bet types
                        dtbResult = ReportDataAccess.Age_Monitor_BetlistSel_Ag_OtherBettypes(custId, transId, limitRowCount);
                        break;
                }
            }

            return dtbResult;
        }

        #endregion For Last Bets Monitor

        #region Cancel Betlist

        public DataTable BetList_SelDeleted(Hashtable htb)
        {
            return ReportDataAccess.BetList_SelDeleted(htb);
        }

        public DataTable CancelBetList_Agent(string custId)
        {
            return ReportDataAccess.CancelBetList_Agent(custId);
        }

        public DataTable CancelBetList_Master(string custId)
        {
            return ReportDataAccess.CancelBetList_Master(custId);
        }

        public DataTable CancelBetList_Super(string custId)
        {
            return ReportDataAccess.CancelBetList_Super(custId);
        }

        #endregion Cancel Betlist

        #region Portal Page

        #region Balance Info

        public DataTable GetBalanceInfo(int custId)
        {
            var date = MoveBetsDate;

            if (UserSession.Current.IsAgent)
            {
                return ReportDataAccess.Age_CustomerSelBalanceInfo_Ag(custId, date);
            }
            else if (UserSession.Current.IsMaster)
            {
                return ReportDataAccess.Age_CustomerSelBalanceInfo_MAg(custId, date);
            }
            else if (UserSession.Current.IsSuper)
            {
                return ReportDataAccess.Age_CustomerSelBalanceInfo_SAg(custId, date);
            }

            return null;
        }

        #endregion Balance Info

        #region Statistic Info

        public DataTable GetStatisticInfo(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return ReportDataAccess.Age_CustomerSelStatisticsInfo_Ag(custId);
            }
            else if (UserSession.Current.IsMaster)
            {
                return ReportDataAccess.Age_CustomerSelStatisticsInfo_MAg(custId);
            }
            else if (UserSession.Current.IsSuper)
            {
                return ReportDataAccess.Age_CustomerSelStatisticsInfo_SAg(custId);
            }

            return null;
        }

        public DataTable GetNewMembers(int custId)
        {
            return ReportDataAccess.Age_CustomerSelNewMembers(custId);
        }

        public DataTable GetTopWinningMembers(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return ReportDataAccess.Age_CustomerSelTopWinningMembers_Ag(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsMaster)
            {
                return ReportDataAccess.Age_CustomerSelTopWinningMembers_MAg(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsSuper)
            {
                return ReportDataAccess.Age_CustomerSelTopWinningMembers_SAg(custId, BalanceUpDate, MoveBetsDate);
            }

            return null;
        }

        public DataTable GetTopTurnoverMembers(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return ReportDataAccess.Age_CustomerSelTopTurnoverMembers_Ag(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsMaster)
            {
                return ReportDataAccess.Age_CustomerSelTopTurnoverMembers_MAg(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsSuper)
            {
                return ReportDataAccess.Age_CustomerSelTopTurnoverMembers_SAg(custId, BalanceUpDate, MoveBetsDate);
            }

            return null;
        }

        #endregion Statistic Info

        #region Online List

        public DataTable GetOnlineList(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return ReportDataAccess.Age_CustomerSelOnlineList_Ag(custId);
            }
            else if (UserSession.Current.IsMaster)
            {
                return ReportDataAccess.Age_CustomerSelOnlineList_MAg(custId);
            }
            else if (UserSession.Current.IsSuper)
            {
                return ReportDataAccess.Age_CustomerSelOnlineList_SAg(custId);
            }

            return null;
        }

        #endregion Online List

        #endregion Portal Page

        #region MixParlay

        /// <summary>
        /// Select matches from BetTransM by Refno
        /// For view MixParlay result
        /// </summary>
        public DataTable SelectMatchResultParlay(long refno)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultParlay(refno);
        }

        /// <summary>
        /// Select matches from BetTransM by Refno
        /// For view MixParlay result
        /// </summary>
        public DataSet SelectMatchResultParlayDS(long refno)
        {
            return ReportDataAccess.Age_BetList_SelMatchResultParlayDS(refno);
        }

        #endregion MixParlay

        #region Bet Summary

        /// <summary>
        /// Get Bet summary list
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<BetSummaryEntity> GetBetSummaryList(int custId, DateTime dateFrom, DateTime dateTo)
        {
            string language = CurrentSession.Language;
            DataTable dataTable = ReportDataAccess.Age_Statement_Mem_BetSummary(custId, dateFrom, dateTo, language);

            List<BetSummaryEntity> betSummaryList = new List<BetSummaryEntity>();
            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    betSummaryList.Add(new BetSummaryEntity(row));
                }
            }

            return betSummaryList;
        }

        #endregion Bet Summary

        private static IEnumerable<MatchDetailInformation> MappingMonitoringBetListFromDatatableToObject(
            DataTable runningMatchs)
        {
            var result = new List<MatchDetailInformation>();

            foreach (DataRow matchInfo in runningMatchs.Rows)
            {
                result.Add(new MatchDetailInformation()
                {
                    MatchId = Converter.ToInt(matchInfo["matchid"]),
                    MatchCode = Converter.ToString2(matchInfo["matchcode"]),
                    AwayId = Converter.ToInt(matchInfo["awayid"]),
                    HomeId = Converter.ToInt(matchInfo["homeid"]),
                    Race = string.IsNullOrWhiteSpace(matchInfo["race"].ToString()) ? 0 : Converter.ToInt(matchInfo["race"]),
                    ShowTime = Convert.ToString(matchInfo["showtime"]),
                    SportType = Converter.ToInt(matchInfo["sporttype"]),
                    LeagueId = Converter.ToInt(matchInfo["leagueid"])
                });
            }

            return result;
        }
    }
}