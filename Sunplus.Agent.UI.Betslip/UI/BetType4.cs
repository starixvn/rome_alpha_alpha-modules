namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Correct score
    /// </summary>
    public class BetType4 : BetType1
    {
        public override void AssignChoice()
        {
            Choice = Cast.AsString(Data["betteam"]);

            Choice = (Choice == "0:5" || Choice == "5:0") ? Choice + " UP" : Choice;
        }

        public override void AssignChoiceLabel()
        {
            ChoiceLabel = LocalResources.BetSlipUILang.lblScore;
        }

        public override void AssignHome()
        {
            var homeTeam = CacheRef.GetTeamName(Data["homeid"]);
            homeTeam += Cast.AsBooleanInvariant(Data["isNeutral"]) ? LocalResources.BetSlipUILang.IsNeutral : string.Empty;
            Home = homeTeam;
        }

        public override void  AssignAway()
        {
            var away = CacheRef.GetTeamName(Data["awayid"]);
            Away = string.Format("<font class=\"b\">{0}</font>", away);
        }

        public override void AssignHdp()
        {
            Hdp = "--";
        }
    }
}
