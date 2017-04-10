namespace Alpha.SmartPunter.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using SmartPunter.Services;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class SmartPunterBetListMonitorTests
    {
        private IUserSession _userSession;
        private ISmartPunterService _smartPunterService;
        private ISmartPunterBetListMonitor _smartPunterBetListMonitor;
        private Func<string> _clientIP;

        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<IUserSession>();
            _smartPunterService = Substitute.For<ISmartPunterService>();
            _clientIP = () => "127.0.0.1";
            _smartPunterBetListMonitor = new SmartPunterBetListMonitor(_userSession, _smartPunterService, _clientIP);
        }

        [Test]
        public void WriteLog_IsInternalFalse_DoNothing()
        {
            // Arrange.
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>();
            bool isInternal = false;
            _userSession.IsInternal.Returns(isInternal);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .DidNotReceive()
                .CheckSmartPunter(customerId);
        }

        [Test]
        public void WriteLog_IsInternalAdminFalse_DoNothing()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>();

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = false;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer();
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .DidNotReceive()
                .CheckSmartPunter(customerId);
        }

        [Test]
        public void WriteLog_IsSmartPunterHasDownlineSmartPunterAreFalse_DoNothing()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>();

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = true;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer();
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .DidNotReceive()
                .HasSmartPunterPermission(userId);
        }

        [Test]
        public void WriteLog_IsSmartPunterTrue_WriteLogNoData()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>();

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = true;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer(true, false);
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            bool isRestricted = false;
            _smartPunterService.HasSmartPunterPermission(userId).Returns(isRestricted);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .Received()
                .WriteLogNoData(customerId, !isRestricted, userId, _clientIP());
        }

        [Test]
        public void WriteLog_IsInternalTrue_HasDownlineSmartPunterTrue_WriteLogNoData()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>();

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = true;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer(false, true);
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            bool isRestricted = false;
            _smartPunterService.HasSmartPunterPermission(userId).Returns(isRestricted);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .Received()
                .WriteLogNoData(null, !isRestricted, userId, _clientIP());
        }

        [Test]
        public void WriteLog_IsInternalTrue_IsSmartPunterTrue_WriteLogUserNameList()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>
            {
                new Ticket { UserName = "member01" },
                new Ticket { UserName = "member011" },
                new Ticket { UserName = "member012" }
            };

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = true;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer(true, false);
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            bool isRestricted = false;
            _smartPunterService.HasSmartPunterPermission(userId).Returns(isRestricted);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .Received()
                .WriteLog(string.Join(",", tickets.GroupBy(grp => grp.UserName).Select(grp => grp.Key)), !isRestricted, userId, _clientIP());
        }

        [Test]
        public void WriteLog_IsInternalTrue_IsSmartPunterTrue_CheckSmartPunterAndWriteLog()
        {
            // Arrange.
            int userId = 8;
            int customerId = 1176070;
            IList<ITicket> tickets = new List<ITicket>
            {
                new Ticket { UserName = "member01" },
                new Ticket { UserName = "member011" },
                new Ticket { UserName = "member012" }
            };

            bool isInternal = true;
            _userSession.IsInternal.Returns(isInternal);

            bool isInternalAdmin = true;
            _smartPunterService.CheckInternalAdmin(userId).Returns(isInternalAdmin);

            var customer = new SMACustomer(false, true);
            _smartPunterService.CheckSmartPunter(customerId).Returns(customer);

            _userSession.AdminId.Returns(userId);

            bool isRestricted = false;
            _smartPunterService.HasSmartPunterPermission(userId).Returns(isRestricted);

            // Act.
            _smartPunterBetListMonitor.WriteLog(customerId, tickets);

            // Assert.
            _smartPunterService
                .Received()
                .CheckSmartPunterAndWriteLog(string.Join(",", tickets.GroupBy(grp => grp.UserName).Select(grp => grp.Key)), !isRestricted, userId, _clientIP());
        }
    }
}