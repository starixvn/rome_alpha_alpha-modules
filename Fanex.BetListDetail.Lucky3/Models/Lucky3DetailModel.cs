namespace Fanex.BetListDetail.Lucky3.Models
{
    using Fanex.BetListDetail.Lucky3.Enums;
    using System.Collections.Generic;
    using System.Linq;

    public class Lucky3DetailModel
    {
        public long TransactionId { get; set; }

        public int MatchId { get; set; }

        public int CustomerId { get; set; }

        public int BetType { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public string TransactionDescription { get; set; }

        public decimal Hdp1 { get; set; }

        public decimal Bets { get; set; }

        public int Hits { get; set; }

        public int Prize { get; set; }

        public decimal PrizeAmount { get; set; }

        public decimal Stake { get; set; }

        public decimal? Winlost { get; set; }

        public decimal? PlayerCommission { get; set; }

        public decimal SuperDiscount { get; set; }

        public decimal AgentDiscount { get; set; }

        public decimal MasterDiscount { get; set; }

        public decimal SuperCommission { get; set; }

        public decimal AgentCommission { get; set; }

        public decimal MasterCommission { get; set; }

        public bool IsWinningShare => BetType == (int)Lucky3BetType.WinningShare;

        public bool IsLosingCombination => BetType == (int)Lucky3BetType.WinLose && StatusId == (int)Lucky3Status.Lose;

        public bool IsPrize => BetType == (int)Lucky3BetType.WinLose && StatusId == (int)Lucky3Status.Won;

        public IEnumerable<string> Numbers => TransactionDescription.Split(',').Select(i => i.Trim());
    }
}