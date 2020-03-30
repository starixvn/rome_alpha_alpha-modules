namespace Fanex.BetDetails.Casino.Tests.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builder;
    using BetList.Core.Entities;
    using Casino.ViewModels;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class CasinoDetailsViewModelTests
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
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel("", 1, 4, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewSuperPT);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void AllowToViewSuperPT_ViewerLevelIsNotSuper_ReturnFalse(int viewerLevel)
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel("", 1, viewerLevel, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.False(viewModel.AllowToViewSuperPT);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void AllowToViewMasterPT_ViewerLevelIsSuperOrMaster_ReturnTrue(int viewerLevel)
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel("", 1, viewerLevel, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewMasterPT);
        }

        [Test]
        public void AllowToViewMasterPT_ViewerLevelIsNotSuperOrMaster_ReturnFalse()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel("", 1, 2, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.False(viewModel.AllowToViewMasterPT);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Title_SportNameIsNullOrEmpty_ReturnRNGCasinoDetails(string sportName)
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel(sportName, 1, 2, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual("RNG Casino Details", viewModel.Title);
        }

        [TestCase("Macau Games")]
        [TestCase("Fishing World")]
        [TestCase("Live Casino")]
        public void Title_SportNameIsSupported_ReturnCorrectTitle(string sportName)
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);
            var expectedTitle = sportName + " Details";


            // Act
            var viewModel = new CasinoDetailsBaseViewModel(sportName, 1, 2, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual(expectedTitle, viewModel.Title);
        }

        [Test]
        public void StartNo_PageIndexIsFirstPage_ReturnOne()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel(string.Empty, 1, 2, param, new PagedData<ITicket>(tickets, 1, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual(1, viewModel.StartNo);
        }

        [Test]
        public void StartNo_PageIndexIsSecondPage_ReturnCorrectValue()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel(string.Empty, 1, 2, param, new PagedData<ITicket>(tickets, 2, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual(101, viewModel.StartNo);
        }

        [Test]
        public void StartNo_PageIndexIsThirdPage_ReturnCorrectValue()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 6, 1, 100);

            // Act
            var viewModel = new CasinoDetailsBaseViewModel(string.Empty, 1, 2, param, new PagedData<ITicket>(tickets, 3, 100, 0), htmlBuilder, ticketHelper);

            // Assert
            Assert.AreEqual(201, viewModel.StartNo);
        }
    }
}
