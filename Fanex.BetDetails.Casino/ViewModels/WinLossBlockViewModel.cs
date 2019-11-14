namespace Fanex.BetDetails.Casino.ViewModels
{
    using BetList.Core.Entities;
    using BetList.Core.Utils;
    using Constants;

    public class WinLossBlockViewModel
    {
        public WinLossBlockViewModel(ITicket ticket)
        {
            SetData(ticket);
            CommVisibility = DisplayComm(ticket);
        }

        public string WinLost { get; private set; }

        public bool CommVisibility { get; }

        public string Comm { get; private set; }

        private static bool DisplayComm(ITicket ticket)
        {
            return ticket.BetTypeId != BetTypeIdConstants.MatchWinner;
        }

        private void SetData(ITicket ticket)
        {
            var status = ticket.Status.ToLowerInvariant();

            switch (status)
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                    WinLost = "-";
                    Comm = string.Empty;
                    break;

                case BetStatus.Void:
                    WinLost = ticket.BetTypeId == BetTypeIdConstants.ChargeCommission ? "(NW) 0.00" : string.Empty;
                    Comm = "0.00";
                    break;

                case BetStatus.Reject:
                    WinLost = "0.00";
                    Comm = string.Empty;
                    break;

                default:
                    WinLost = ticket.BetTypeId == BetTypeIdConstants.ChargeCommission ? "(NW) " + ConvertByBetType.Winloss(ticket.ActualStake) : ConvertByBetType.Winloss(ticket.Winlost);
                    Comm = ticket.BetTypeId == BetTypeIdConstants.ChargeCommission ? ConvertByBetType.Winloss(ticket.PlayerComm) : ConvertByBetType.Comm(ticket.PlayerComm);
                    break;
            }
        }
    }
}
