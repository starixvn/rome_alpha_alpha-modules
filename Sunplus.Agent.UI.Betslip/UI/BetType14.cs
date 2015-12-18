namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.UI.BetSlip.Utils;
    using Sunplus.Agent.Utilities;
    /// <summary>
    /// First Goal/Last Goal
    /// </summary>
    public class BetType14 : BetType5
    {
        public override void AssignChoice()
        {
            Choice = ConvertOddsByBetTypes.GetBetTeam(Data["betteam"].ToString(), Cast.AsInt32(Data["bettype"]));
        }
    }
}
