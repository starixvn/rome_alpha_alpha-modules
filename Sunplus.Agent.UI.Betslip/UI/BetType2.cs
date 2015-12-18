namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Odd/Even
    /// </summary>
    public class BetType2 : BetType1
    {
        public override void AssignChoice()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            Choice = betTeam == "h" ? LocalResources.BetSlipUILang.odd.ToUpper() : LocalResources.BetSlipUILang.even;
        }

        public override void AssignHdp()
        {
            Hdp = "--";
        }
    }
}
