namespace Fanex.BetListDetail.Casino.Tests.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builder;
    using BetList.Core.Entities;
    using Casino.ViewModels;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoDetailViewModelTests
    {
        private IBetListHTMLBuilder htmlBuilder;
        private ITicketHelper ticketHelper;
        private IEnumerable<ITicket> tickets;

        [SetUp]
        public void SetUp()
        {
            htmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            ticketHelper = Substitute.For<ITicketHelper>();
            tickets = new List<ITicket>();
        }

        [Test]
        public void AllowToViewSuperPT_ViewerLevelIsSuper_ReturnTrue()
        {
            // Act
            var viewModel = new CasinoDetailViewModel("", 1, 4, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewSuperPT);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void AllowToViewSuperPT_ViewerLevelIsNotSuper_ReturnFalse(int viewerLevel)
        {
            // Act
            var viewModel = new CasinoDetailViewModel("", 1, viewerLevel, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.False(viewModel.AllowToViewSuperPT);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void AllowToViewMasterPT_ViewerLevelIsSuperOrMaster_ReturnTrue(int viewerLevel)
        {
            // Act
            var viewModel = new CasinoDetailViewModel("", 1, viewerLevel, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewMasterPT);
        }

        [Test]
        public void AllowToViewMasterPT_ViewerLevelIsNotSuperOrMaster_ReturnFalse()
        {
            // Act
            var viewModel = new CasinoDetailViewModel("", 1, 2, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.False(viewModel.AllowToViewMasterPT);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Title_SportNameIsNullOrEmpty_ReturnRNGCasinoDetails(string sportName)
        {
            // Act
            var viewModel = new CasinoDetailViewModel(sportName, 1, 2, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual("RNG Casino Details", viewModel.Title);
        }

        [TestCase("Macau Games")]
        [TestCase("Fishing World")]
        [TestCase("Allbet")]
        public void Title_SportNameIsSupported_ReturnCorrectTitle(string sportName)
        {
            // Arrange
            var expectedTitle = sportName + " Details";

            // Act
            var viewModel = new CasinoDetailViewModel(sportName, 1, 2, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual(expectedTitle, viewModel.Title);
        }
    }
}
