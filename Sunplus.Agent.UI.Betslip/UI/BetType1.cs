namespace Sunplus.Agent.UI.BetSlip.UI
{
    using System;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetSlip.Utils;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Handicap Bet Type (Shows or hides columns base on Bet List Type)
    /// <remarks>This class is base class for other Bet type classes to inherit</remarks>
    /// </summary>
    public class BetType1 : BetTypeBase
    {
        #region First Line

        /// <summary>
        /// Assign Home VS Away
        /// </summary>
        public override void AssignHome()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            var homeTeam = CacheRef.GetTeamName(Data["homeid"]);
            homeTeam += Cast.AsBooleanInvariant(Data["isNeutral"]) ? LocalResources.BetSlipUILang.IsNeutral : string.Empty;

            Home = betTeam == "h"
                       ? string.Format("<font class=\"b\">{0}</font>", homeTeam)
                       : homeTeam;
        }

        /// <summary>
        /// Assign Home VS Away
        /// </summary>
        public override void AssignAway()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            var away = CacheRef.GetTeamName(Data["awayid"]);
            Away = betTeam != "h" ? string.Format("<font class=\"b\">{0}</font>", away) : away;
        }

        public override void AssignChoiceLabel()
        {
            ChoiceLabel = LocalResources.BetSlipUILang.choice;
        }

        public override void AssignChoice()
        {
            var betTeam = Cast.AsString(Data["betteam"]);

            var homeTeam = CacheRef.GetTeamName(Data["homeid"]);
            var away = CacheRef.GetTeamName(Data["awayid"]);

            Choice = betTeam == "h" ? homeTeam : away;
        }

        public override void AssignHdp()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            var hdp1 = Cast.AsDecimal(Data["hdp1"]);
            var hdp2 = Cast.AsDecimal(Data["hdp2"]);
            Hdp = betTeam == "h"
                      ? Formater.DecFormat((hdp2 - hdp1), 2)
                      : Formater.DecFormat((hdp1 - hdp2), 2);
        }

        #endregion First Line

        #region Second Line

        public override void AssignPotentialWinning()
        {
            var bettype = Cast.AsInt32(Data["bettype"]);
            var getOddsType = DataHelper.GetValueFromDataRow(Data, ColumnName.OddsType);
            var oddsType = 0;
            if (getOddsType != null)
            {
                oddsType = Cast.AsInt32(getOddsType);
            }

            var odds = Convert.ToDouble(DataHelper.GetValueFromDataRow(Data, ColumnName.Odds));

            odds = ConvertOddsByBetTypes.GetOddsByBetType(odds, bettype, oddsType);

            var stake = Cast.AsDecimal(DataHelper.GetValueFromDataRow(Data, ColumnName.Stake));
            var potentialWining = GetPotentialWinning(bettype, oddsType, (decimal)odds, stake);
            PotentialWinning = Formater.FormatNumber2(potentialWining, null);
        }

        public override void AssignWinning()
        {
            var winning = DataHelper.GetValueFromDataRow(Data, ColumnName.WinLost);
            Winning = Formater.FormatNumber2(winning, null);
        }

        public override void AssignIsLiveLabel()
        {
            var isLive = Convert.ToBoolean(Data["liveindicator"]);
            IsLiveBettingLabel = isLive ? LocalResources.BetSlipUILang.lblLiveBetting : LocalResources.BetSlipUILang.lblNotLiveBetting;
        }

        public override void AssignIsLiveValue()
        {
            var isLive = Convert.ToBoolean(Data["liveindicator"]);
            IsLiveValue = isLive ? Data["liveindicator"].ToString() : "---";
        }

        public override void AssignLiveScore()
        {
            LiveScore = Convert.ToString(Data["livehomescore"]) + " - " + Convert.ToString(Data["liveawayscore"]);
        }

        public override void AssignResultTime()
        {
            ResultTime = Formater.ShortDateFormat(Cast.AsDateTime(Data["winlostdate"]));
        }

        public override void AssignFinalScore()
        {
            FinalScore = Cast.AsString(Data["finalhomescore"]) + " - " +
                         Cast.AsString(Data["finalawayscore"]);
        }

        public override void AssignFinalScoreLabel()
        {
            FinalScoreLabel = LocalResources.BetSlipUILang.finalscore;
        }

        public override void AssignBetSlipCode()
        {
            var transid = Data["transid"].ToString();
            RefNoEncode = Sunplus.Agent.Utilities.Encryption.CfsEnCode(transid + "99");
        }

        #endregion Second Line
    }
}