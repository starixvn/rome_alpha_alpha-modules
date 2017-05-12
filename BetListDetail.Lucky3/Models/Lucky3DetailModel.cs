using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanex.BetListDetail.Lucky3.Models
{
    public class Lucky3DetailModel
    {
        public long TransactionId { get; set; }

        public int MatchId { get; set; }

        public int Customerid { get; set; }

        public int BetType { get; set; }

        public int StatusId { get; set; }

        public string TransactionDescription { get; set; }

        public decimal Hdp1 { get; set; }

        public decimal Bets { get; set; }

        public int Hits { get; set; }

        public int Prize { get; set; }

        public decimal PrizeAmount { get; set; }

        public decimal Stake { get; set; }

        public decimal? Winlost { get; set; }

        public decimal? PlayerCommition { get; set; }

        public decimal SuperDiscount { get; set; }

        public decimal AgentDiscount { get; set; }

        public decimal Discount { get; set; }

        public decimal SuperCommition { get; set; }

        public decimal AgentCommition { get; set; }

        public decimal Commmition { get; set; }
    }
}