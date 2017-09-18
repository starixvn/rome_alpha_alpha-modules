namespace Fanex.BetDetails.Casino.ViewModels
{
    using BetList.Core.Entities;
    using BetList.Core.Utils;

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
            var isIMBCMatch = ticket.SuperDiscount < 0 || ticket.MasterDiscount < 0 || ticket.AgentDiscount < 0;

            return ticket.BetTypeId != 501 || !isIMBCMatch;
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
                    WinLost = ticket.BetTypeId == 500 ? "(NW) 0.00" : string.Empty;
                    Comm = "0.00";
                    break;

                case BetStatus.Reject:
                    WinLost = "0.00";
                    Comm = string.Empty;
                    break;

                default:
                    WinLost = ticket.BetTypeId == 500 ? "(NW) " + ConvertByBetType.Winloss(ticket.ActualStake) : ConvertByBetType.Winloss(ticket.Winlost);
                    Comm = ticket.BetTypeId == 500 ? ConvertByBetType.Winloss(ticket.PlayerComm) : ConvertByBetType.Comm(ticket.PlayerComm);
                    break;
            }
        }
    }
}
