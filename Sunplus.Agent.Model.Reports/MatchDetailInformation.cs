namespace Sunplus.Agent.Model.Entities.BetListMonitoring
{
    using System;

    [Serializable]
    [Obsolete("Should rename to Sunplus.Agent.Model.Reports")]
    public class MatchDetailInformation
    {
        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int Race { get; set; }

        public string ShowTime { get; set; }
    }
}