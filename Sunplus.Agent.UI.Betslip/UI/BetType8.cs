namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// 1st Over/Under
    /// </summary>
    public class BetType8 : BetType3
    {
        public override void  AssignFinalScore()
        {
            FinalScore = Cast.AsString(Data["hthomescore"]) + " - " +
                         Cast.AsString(Data["htawayscore"]);
        }

        public override void AssignFinalScoreLabel()
        {
            FinalScoreLabel = LocalResources.BetSlipUILang.firsthalfscore;
        }
    }
}
