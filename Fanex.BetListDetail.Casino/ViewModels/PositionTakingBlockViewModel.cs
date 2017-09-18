namespace Fanex.BetListDetail.Casino.ViewModels
{
    using BetList.Core.Utils;

    public class PositionTakingBlockViewModel
    {
        public PositionTakingBlockViewModel(decimal positionTaking, decimal winLoss, string commStatus, decimal discount, decimal comm)
        {
            PositionTakingPercent = ConvertByBetType.Pt(positionTaking) + "%";
            WinLoss = ConvertByBetType.PtValue(winLoss);
            CommStatus = commStatus;
            CommissionPercent = GetCommisionPercentText(ConvertByBetType.Comm_Percentage(discount));
            Commission = ConvertByBetType.Comm(comm);
        }

        public string PositionTakingPercent { get; }

        public string WinLoss { get; }

        public string CommStatus { get; }

        public string CommissionPercent { get; }

        public string Commission { get; }

        private string GetCommisionPercentText(string commPercent)
            => CommStatus == "1" ? $"2x {commPercent}%" : $"{commPercent}%";
    }
}
