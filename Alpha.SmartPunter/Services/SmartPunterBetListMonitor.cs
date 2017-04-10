namespace Alpha.SmartPunter.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;

    public class SmartPunterBetListMonitor : ISmartPunterBetListMonitor
    {
        private readonly IUserSession _userSession;
        private readonly ISmartPunterService _smartPunterService;
        private readonly Func<string> _clientIP;

        public SmartPunterBetListMonitor(
            IUserSession userSession,
            ISmartPunterService smartPunterService)
            : this(userSession, smartPunterService, null)
        {
        }

        public static ISmartPunterBetListMonitor CreateInstance(
            IUserSession userSession,
            ISmartPunterService smartPunterService,
            Func<string> clientIP)
        {
            return new SmartPunterBetListMonitor(userSession, smartPunterService, clientIP);
        }

        internal SmartPunterBetListMonitor(
            IUserSession userSession,
            ISmartPunterService smartPunterService,
            Func<string> clientIP)
        {
            _userSession = userSession ?? UserSession.Current;
            _clientIP = clientIP ?? AgentIPFormat.GetClientIP;
            _smartPunterService = smartPunterService;
        }

        public void WriteLog(int customerId, IList<ITicket> tickets)
        {
            int userId = _userSession.AdminId;

            if (!_userSession.IsInternal || !_smartPunterService.CheckInternalAdmin(userId))
            {
                return;
            }

            bool isSmartPunter = false;
            bool hasDownlineSmartPunter = false;
            var customer = _smartPunterService.CheckSmartPunter(customerId);

            if (customer != null)
            {
                isSmartPunter = customer.IsSmartPunter;
                hasDownlineSmartPunter = customer.HasDownlineSmartPunter;
            }

            if (!isSmartPunter && !hasDownlineSmartPunter)
            {
                return;
            }

            bool isRestricted = !_smartPunterService.HasSmartPunterPermission(userId);

            if (tickets == null || tickets.Count == 0)
            {
                if (isSmartPunter)
                {
                    _smartPunterService.WriteLogNoData(customerId, isRestricted, userId, _clientIP());
                }
                else if (hasDownlineSmartPunter)
                {
                    _smartPunterService.WriteLogNoData(null, isRestricted, userId, _clientIP());
                }
            }
            else
            {
                string userNames = GetUserNamesFromTickets(tickets);

                if (isSmartPunter)
                {
                    _smartPunterService.WriteLog(userNames, isRestricted, userId, _clientIP());
                }
                else if (hasDownlineSmartPunter)
                {
                    _smartPunterService.CheckSmartPunterAndWriteLog(userNames, isRestricted, userId, _clientIP());
                }
            }
        }

        private string GetUserNamesFromTickets(IList<ITicket> tickets)
        {
            return string.Join(",", tickets.GroupBy(grp => grp.UserName).Select(grp => grp.Key));
        }
    }
}