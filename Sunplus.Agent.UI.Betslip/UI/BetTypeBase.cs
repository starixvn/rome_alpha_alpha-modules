namespace Sunplus.Agent.UI.BetSlip.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetSlip.Utils;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Base Bet Type class
    /// </summary>
    public abstract class BetTypeBase : BetType1_Template, IBetItem
    {       
        #region Constructor

        #endregion

        #region Abstract Members

        public abstract void AssignHome();
        public abstract void AssignAway();
        public abstract void AssignChoice();
        public abstract void AssignChoiceLabel();
        public abstract void AssignHdp();
        public abstract void AssignWinning();
        public abstract void AssignIsLiveLabel();
        public abstract void AssignIsLiveValue();
        public abstract void AssignLiveScore();
        public abstract void AssignResultTime();
        public abstract void AssignFinalScore();
        public abstract void AssignFinalScoreLabel();
        public abstract void AssignBetSlipCode();
        public abstract void AssignPotentialWinning();

        #endregion

        #region Virtual

        public virtual void AssignBetId()
        {
            BetId = Data["transid"].ToString();
        }

        public virtual void AssignTransType()
        {
            TransTime = Formater.ShortDateFormat(Data["transdate"]) + "<br/>" +
                        Cast.AsDateTime(Data["transdate"]).ToString("hh:mm:ss tt");
        }

        public virtual void AssignMatchCode()
        {
            MatchCode = Cast.AsString(Data["matchcode"]);
        }

        public virtual void AssignVs()
        {
            var isLive = Cast.AsBooleanInvariant(Data["liveindicator"]);
            var betType = Cast.AsInt32(Data["bettype"]);
            VS = betType == 10 && !isLive ? "vs" : "-vs-";
        }

        public virtual void AssignOdds()
        {
            double odds = ConvertOddsByBetTypes.GetOddsByBetType(Convert.ToDouble(Data["odds"]), Convert.ToInt32(Data["bettype"]), Convert.ToInt32(Data["oddstype"]));
            Odds = Formater.FormatNumber3(odds, null);
            OddsType = CacheRef.GetOddsTypeName(ConvertOddsByBetTypes.ShowOddsByBettype(Convert.ToInt32(Data["bettype"]), Convert.ToInt32(Data["oddstype"])));
        }

        public virtual void AssignStake()
        {
            var stake = DataHelper.GetValueFromDataRow(Data, ColumnName.Stake);
            Stake = Formater.FormatNumber2(stake, null);
        }

        public virtual void AssignStatus()
        {
            var status = Cast.AsString(Data["status"]);
            Status = GetStatus(status);
        }

        #endregion

        #region Protected

        protected void AssignBetChoice()
        {
            AssignBetId();
            AssignTransType();
            AssignMatchCode();
            AssignHome();
            AssignAway();
            AssignVs();
            AssignChoice();
            AssignChoiceLabel();
            AssignHdp();
            AssignOdds();
            AssignStake();
            AssignStatus();
        }

        protected void AssignBetResult()
        {
            AssignPotentialWinning();
            AssignWinning();
            AssignIsLiveLabel();
            AssignIsLiveValue();
            AssignLiveScore();
            AssignResultTime();
            AssignFinalScoreLabel();
            AssignFinalScore();
        }

        #endregion
      
        #region IBetItem

        public void AssignData()
        {
            AssignBetChoice();
            AssignBetResult();
            AssignBetSlipCode();
        }

        public DataRow Data { get; set; }

        #endregion

        #region IComponent
        
        public virtual void HideLinkedResources()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utility

        private readonly IDictionary<string, string> _statusMap = new Dictionary<string, string>
        {
            {"running", LocalResources.BetSlipUILang.running},
            {"waiting", LocalResources.BetSlipUILang.waiting},
            {"draw", LocalResources.BetSlipUILang.lblDraw},
            {"lose", LocalResources.BetSlipUILang.lose},
            {"won", LocalResources.BetSlipUILang.won},
            {"void", LocalResources.BetSlipUILang.voided}
        };

        protected string GetStatus(string status)
        {
            return _statusMap.ContainsKey(status.ToLower()) ? _statusMap[status.ToLower()] : status;
        }


        /// <summary>
        /// Method use calculate potential winning
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="oddsType"></param>
        /// <param name="odds"></param>
        /// <param name="stake"></param>
        /// <returns></returns>
        public virtual decimal GetPotentialWinning(int betType, int oddsType, decimal odds, decimal stake)
        {
            if (IsCapitalBetType(betType))
            {
                return (odds - 1) * stake;
            }

            if (odds <= 0)
            {
                return stake;
            }

            if (oddsType == 5) return (odds * stake) / 100;
            if (oddsType == 1) return (odds - 1) * stake;

            return odds * stake;
        }

        private readonly IDictionary<int, bool> _mapIsCapital = new Dictionary<int, bool>
        {
            {1, false},
            {2, false},
            {3, false},
            {7, false},
            {8, false},
            {12, false},
            {20, false},
            {121, false},
            {122, false},
            {123, false},
            {124, false},
            {125, false},
            {21, false},
            {81, false},
            {82, false},
            {83, false},
            {84, false},
            {85, false},
            {86, false},
            {87, false},
            {88, false},
            {89, false},
            {90, false}
        };

        /// <summary>
        /// Method use return bool for capital bettype
        /// </summary>
        /// <param name="betType"></param>
        /// <returns>true or false</returns>
        private bool IsCapitalBetType(int betType)
        {
            return !_mapIsCapital.ContainsKey(betType) || _mapIsCapital[betType];
        }

        protected string FormatSignNumber(int number)
        {
            if (number > 0) return "+" + number;
            return number.ToString();
        }

        #endregion

    }
}
