namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.Entities.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.Reports")]
    public class BetSummaryEntity : IEntity    
    {
        public BetSummaryEntity() 
        { 
        }

        public BetSummaryEntity(DataRow dataRow)            
        {
            this.EventDate = (DateTime)dataRow["eventdate"];
            this.BetType = Convert.ToInt32(dataRow["bettype"]);
            this.MatchCode = dataRow["matchcode"] as string;
            this.HomeId = (int)dataRow["homeid"];
            this.AwayId = (int)dataRow["awayid"];
            this.Status = dataRow["status"] as string;
            this.TotalBet = (decimal)dataRow["totalbet"];
            this.TotalStake = (decimal)dataRow["totalstake"];
            this.Winning = (decimal)dataRow["winning"];
            this.BetTeam = dataRow["betteam"] as string;
            this.IsNeutral = (bool)dataRow["IsNeutral"];
        }

        public DateTime EventDate { get; set; }

        public int BetType { get; set; }

        public string MatchCode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string Status { get; set; }

        public decimal TotalBet { get; set; }

        public decimal TotalStake { get; set; }

        public decimal Winning { get; set; }

        public string BetTeam { get; set; }

        public bool IsNeutral { get; set; }
    }
}
