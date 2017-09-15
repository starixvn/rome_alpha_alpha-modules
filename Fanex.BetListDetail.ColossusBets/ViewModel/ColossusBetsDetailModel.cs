
namespace Fanex.BetListDetail.ColossusBets.ViewModel
{
    using Constants;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetListDetail.ColossusBets.Models;
    using System.Collections.Generic;

    public class ColossusBetsDetailModel
    {
        public ColossusBetsDetailModel(MemberChoiceResultModel memberChoice, IEnumerable<ITicket> ticketDetails)
        {
            MemberChoice = memberChoice;
            TicketDetails = ticketDetails;

            CalculateTotal();
        }

        public MemberChoiceResultModel MemberChoice { get; }

        public IEnumerable<ITicket> TicketDetails { get; }

        public decimal TotalWinLoss { get; private set; }

        public decimal TotalSuperCommission { get; private set; }

        public decimal TotalMasterCommission { get; private set; }


        public decimal TotalAgentCommission { get; private set; }

        public decimal WinLossCashOut { get; private set; }

        private decimal ownPercent;
        public int OwnPercent
        {
            get
            {
                return (int)(100 - ownPercent);
            }
        }

        private void CalculateTotal()
        {
            foreach (var ticket in TicketDetails)
            {
                var status = ticket.Status.ToLower();

                TotalWinLoss += ticket.Winlost;
                TotalSuperCommission += status == "running" ? 0 : ticket.SuperComm;
                TotalMasterCommission += status == "running" ? 0 : ticket.AgentComm;
                TotalAgentCommission += status == "running" ? 0 : ticket.Comm;

                if (ticket.BetTypeId == BetType.ColossusBetsCashOut)
                {
                    WinLossCashOut += ticket.Winlost;
                    ownPercent += ticket.Handicap1 * 100;
                }
            }
        }

    }
}
