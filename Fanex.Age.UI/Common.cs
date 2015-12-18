namespace Fanex.Age.UI
{
    public class Common
    {
        // Background color setting
        // Note that we use bgColor, don't use className because performance problem
        public const string RowBgOdd = "#F6F8F9";
        public const string RowBgEven = "#faf9ee";
        public const string RowBgOver = "#f8ff8d";
        public const string RowBgClosed = "#ccc7ba";
        public const string RowBgSuspended = "#f8ded7";
        public const string RowBgLive = "#ffddcc";
        public const string RowBgDisabled = "#A0A97A";

        public const string RowCssOdd = "BgOdd";
        public const string RowCssEven = "BgEven";
        public const string RowCssOver = "BgOver";
        public const string RowCssClosed = "BgClo";
        public const string RowCssSuspended = "BgSus";
        public const string RowCssLive = "BgLive";
        public const string RowCssDisabled = "BgDis";

        // Checked status for checkbox
        public const string Checked = " checked";
        public const string Disabled = " disabled";

        #region BetList
        /// <summary>
        /// BestList Winloss by date
        /// </summary>
        public const string BetListWinLossByDate = "WLByDate";

        /// <summary>
        /// BestList Winloss by date old
        /// </summary>
        public const string BetListWinLossByDateOld = "WLByDate_Old";

        /// <summary>
        /// WinLossByBettypeMatch
        /// </summary>
        public const string WinLossByBetTypeMatch = "WLByBettypeMatch";

        /// <summary>
        /// BetList BettypeMatch (Hdp_OU_Live_TG_OE_Outright_ML)
        /// </summary>
        public const string BetListByBetTypeMatch = "BettypeMatch";

        /// <summary>
        /// BetList BettypeMatch (RACING)
        /// </summary>
        public const string RacingBetListByBetTypeMatch = "RacingBettypeMatch";

        /// <summary>
        /// BetList RunningBettypeMatch
        /// </summary>
        public const string RunningBetListByBetTypeMatch = "RunningBettypeMatch";

        /// <summary>
        /// BestList by Bettype
        /// 1X2_CS_MP
        /// </summary>
        public const string BetListByBetType = "Bettype";

        public const string BestListByMixParlay = "MixParlay";

        public const string BetListByMixParlayOfMatch = "MixParlayByMatch";

        /// <summary>
        /// BetListRunningByDate_Mem
        /// Only for agent - (Member Outstanding)
        /// </summary>
        public const string BetListRunningByDateMember = "RunByDate_Mem";

        /// <summary>
        /// BetListRunningByBettypeMatch_Mem
        /// Only for agent - (Member Outstanding)
        /// </summary>
        public const string BetListRunningByBetTypeMatchMember = "RunByBtypeM";

        /// <summary>
        /// Age_BetList_SelAllByDate_Mem
        /// Only for agent - (Member Outstanding)
        /// </summary>
        public const string BetListByAllDate = "AllDate";

        /// <summary>
        /// Best List Select Deleted
        /// </summary>
        public const string BetListByDeleted = "Deleted";

        /// <summary>
        /// Best List By Daily Statement
        /// </summary>
        public const string BetListByDailyStatement = "BLDailyST";
        #endregion
    }
}
