namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;
    /// <summary>
    /// Half TIME 1x2 HDP
    /// </summary>
    public class BetType125 : BetType1
    {
        public override void AssignFinalScore()
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