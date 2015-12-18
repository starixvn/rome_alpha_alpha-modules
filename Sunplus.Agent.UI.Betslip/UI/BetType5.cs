namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Utilities;
    /// <summary>
    /// FT. 1x2
    /// </summary>
    public class BetType5 : BetType4
    {
        public override void AssignHome()
        {
            var homeTeam = CacheRef.GetTeamName(Data["homeid"]);
            homeTeam += Cast.AsBooleanInvariant(Data["isNeutral"]) ? LocalResources.BetSlipUILang.IsNeutral : string.Empty;
            Home = homeTeam;
        }

        public override void AssignAway()
        {
            var awayTeam = CacheRef.GetTeamName(Data["awayid"]);
            Away = string.Format("<font class=\"b\">{0}</font>", awayTeam);
        }
    }
}
