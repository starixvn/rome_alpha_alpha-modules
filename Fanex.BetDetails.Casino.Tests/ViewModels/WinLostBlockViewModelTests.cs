namespace Fanex.BetDetails.Casino.Tests.ViewModels
{
    using BetList.Core.Entities;
    using BetList.Core.Utils;
    using Casino.ViewModels;
    using Constants;
    using NUnit.Framework;

    [TestFixture]
    public class WinLossBlockViewModelTests
    {
        [TestCase(BetStatus.Running)]
        [TestCase(BetStatus.Waiting)]
        public void WinLost_StatusIsRunningOrWaiting_ShowHyphen(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual("-", viewModel.WinLost);
        }

        [TestCase(BetTypeIdConstants.ChargeCommission, "(NW) 0.00")]
        [TestCase(1, "")]
        public void WinLost_StatusIsVoid_ShowValue(int betTypeId, string expectedWinLost)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.BetTypeId = betTypeId;
            ticket.Status = BetStatus.Void;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual(expectedWinLost, viewModel.WinLost);
        }

        [Test]
        public void WinLost_StatusIsReject_ShowValue()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.Status = BetStatus.Reject;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual("0.00", viewModel.WinLost);
        }

        [TestCase(BetStatus.Won)]
        [TestCase(BetStatus.Lose)]
        [TestCase(BetStatus.Draw)]
        [TestCase(BetStatus.Refund)]
        public void WinLost_Default_BetTypeIsChargeCommission_ShowValue(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.ActualStake = 100;
            ticket.BetTypeId = BetTypeIdConstants.ChargeCommission;
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual($"(NW) {ConvertByBetType.Winloss(ticket.ActualStake)}", viewModel.WinLost);
        }

        [TestCase(BetStatus.Won)]
        [TestCase(BetStatus.Lose)]
        [TestCase(BetStatus.Draw)]
        [TestCase(BetStatus.Refund)]
        public void WinLost_Default_BetTypeIsNotChargeCommission_ShowValue(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.Winlost = 101;
            ticket.BetTypeId = 1;
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual(ConvertByBetType.Winloss(ticket.Winlost), viewModel.WinLost);
        }

        [TestCase(BetStatus.Running)]
        [TestCase(BetStatus.Waiting)]
        [TestCase(BetStatus.Reject)]
        public void Comm_StatusIsRunningOrWaitingOrReject_ShowEmpty(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual(string.Empty, viewModel.Comm);
        }

        [Test]
        public void Comm_StatusIsVoid_ShowEmpty()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.Status = BetStatus.Void;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual("0.00", viewModel.Comm);
        }

        [TestCase(BetStatus.Won)]
        [TestCase(BetStatus.Lose)]
        [TestCase(BetStatus.Draw)]
        [TestCase(BetStatus.Refund)]
        public void Comm_Default_BetTypeIsChargeCommission_ShowValue(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.BetTypeId = BetTypeIdConstants.ChargeCommission;
            ticket.PlayerComm = 500;
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual(ConvertByBetType.Winloss(ticket.PlayerComm), viewModel.Comm);
        }

        [TestCase(BetStatus.Won)]
        [TestCase(BetStatus.Lose)]
        [TestCase(BetStatus.Draw)]
        [TestCase(BetStatus.Refund)]
        public void Comm_Default_BetTypeIsNotChargeCommission_ShowValue(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.BetTypeId = 1;
            ticket.PlayerComm = 500;
            ticket.Status = status;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.AreEqual(ConvertByBetType.Comm(ticket.PlayerComm), viewModel.Comm);
        }

        [Test]
        public void CommVisibility_BetTypeIdIsMatchWinner_IsNotIMBCMatch_ShowValue()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 1;
            ticket.AgentDiscount = 1;
            ticket.BetTypeId = BetTypeIdConstants.MatchWinner;
            ticket.Status = BetStatus.Won;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.True(viewModel.CommVisibility);
        }

        [Test]
        public void CommVisibility_BetTypeIdIsMatchWinner_IsIMBCMatch_ShowValue()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.SuperDiscount = -1;
            ticket.MasterDiscount = 1;
            ticket.AgentDiscount = 1;
            ticket.BetTypeId = BetTypeIdConstants.MatchWinner;
            ticket.Status = BetStatus.Won;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.False(viewModel.CommVisibility);
        }

        [Test]
        public void CommVisibility_BetTypeIdIsNotMatchWinner_ShowTrue()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.BetTypeId = 1;
            ticket.Status = BetStatus.Won;

            // Act
            var viewModel = new WinLossBlockViewModel(ticket);

            // Assert
            Assert.True(viewModel.CommVisibility);
        }
    }
}
