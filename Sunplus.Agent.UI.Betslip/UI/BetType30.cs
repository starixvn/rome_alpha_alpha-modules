namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// 1st Correct Score
    /// </summary>
    public class BetType30 : BetType4
    {
        public override void AssignChoice()
        {
            Choice = Cast.AsString(Data["betteam"]);

            Choice = (Choice == "0:4" || Choice == "4:0") ? Choice + " UP" : Choice;
        }
    }
}
