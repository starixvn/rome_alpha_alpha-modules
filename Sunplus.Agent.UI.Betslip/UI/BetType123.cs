namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Draw no draw
    /// </summary>
    public class BetType123 : BetType1
    {
        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betTeam"].ToString();
            Choice = betTeam.Contains("h") ? LocalResources.BetSlipUILang.lblDraw : LocalResources.BetSlipUILang.lblNoDraw;
        }
    }
}