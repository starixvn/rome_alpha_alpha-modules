namespace Alpha.SmartPunter.Services
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public interface ISmartPunterBetListMonitor
    {
        void WriteLog(int customerId, IList<ITicket> tickets);
    }
}