namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;
    /// <summary>
    /// Over/Under
    /// </summary>
    public class BetType3 : BetType1
    {
        public override void AssignChoice()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            Choice = betTeam == "h" ? LocalResources.BetSlipUILang.over : LocalResources.BetSlipUILang.under;
        }

        public override void AssignHdp()
        {
            var hdp1 = Cast.AsDecimal(Data["hdp1"]);
            Hdp = Formater.DecFormat(hdp1, 2);
        }
    }
}
