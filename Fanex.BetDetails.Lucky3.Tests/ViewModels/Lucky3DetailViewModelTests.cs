using Fanex.BetDetails.Lucky3.Models;
using Fanex.BetDetails.Lucky3.ViewModels;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.BetDetails.Lucky3.Tests.ViewModels
{
    public class Lucky3DetailViewModelTests
    {
        private IEnumerable<Lucky3DetailModel> details;
        [SetUp]
        public void SetUp()
        {
            details = Substitute.For<IEnumerable<Lucky3DetailModel>>();
        }

        [Test]
        public void IsAgent_Level2_ExpectedTrue()
        {
            //// Arrange            
            var viewModel = new Lucky3DetailViewModel(details, 2);

            //// Act
            var act = viewModel.IsAgent;

            //// Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void IsMaster_Level3_ExpectedTrue()
        {
            //// Arrange            
            var viewModel = new Lucky3DetailViewModel(details, 3);

            //// Act
            var act = viewModel.IsMaster;

            //// Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void IsSuper_Level4_ExpectedTrue()
        {
            //// Arrange            
            var viewModel = new Lucky3DetailViewModel(details, 4);

            //// Act
            var act = viewModel.IsSuper;

            //// Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void FormatWinLost_NullWinLost_ReturnEmpty()
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());

            //// Act
            var act = viewModel.FormatWinLost(null);

            //// Assert
            Assert.IsEmpty(act);
        }

        [Test]
        public void FormatWinLost_NegativeWinLost_ReturnRedFontTag()
        {
            //// Arrange
            var negativeWinLost = -1;
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());
            var expected = $"<font color='#B50000'>{negativeWinLost}.00</font>";

            //// Act
            var act = viewModel.FormatWinLost(negativeWinLost);

            //// Assert
            Assert.AreEqual(expected, act);
        }

        [TestCase(1, "1.00")]
        [TestCase(100, "100.00")]
        [TestCase(1000, "1,000.00")]
        public void FormatWinLost_PositiveWinLost_ReturnCorrectFormat(decimal positiveWinLost, string expected)
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());

            //// Act
            var act = viewModel.FormatWinLost(positiveWinLost);

            //// Assert
            Assert.AreEqual(expected, act);
        }

        [TestCase(1, "1.00")]
        [TestCase(100, "100.00")]
        [TestCase(1000, "1,000.00")]
        public void FormatMoney_ReturnCorrectFormat(decimal stake, string expected)
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());

            //// Act
            var act = viewModel.FormatWinLost(stake);

            //// Assert
            Assert.AreEqual(expected, act);
        }

        [TestCase(1.5, "1.5")]
        [TestCase(0.05, "0.05")]
        [TestCase(0.005, "0.005")]
        [TestCase(0.0005, "0.0005")]
        [TestCase(0.00051, "0.0005")]
        public void FormatCommission_ExpectedCorrectFormate(decimal commission, string expected)
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());

            //// Act
            var act = viewModel.FormatCommission(commission);

            //// Assert
            Assert.AreEqual(expected, act);
        }

        [TestCase(0.1, "10%")]
        [TestCase(0.05, "5%")]
        [TestCase(0.025, "2.5%")]
        public void FormatDiscount_ExpectedCorrectFormate(decimal discount, string expected)
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, Arg.Any<int>());

            //// Act
            var act = viewModel.FormatDiscount(discount);

            //// Assert
            Assert.AreEqual(expected, act);
        }

        [Test]
        public void TotalColumns_Level4_Return10Columns()
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, 4);
            var expectedColumns = 10;
            //// Act
            var act = viewModel.TotalColumns;

            //// Assert
            Assert.AreEqual(expectedColumns, act);
        }

        [Test]
        public void TotalColumns_Level3_Return9Columns()
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, 3);
            var expectedColumns = 9;
            //// Act
            var act = viewModel.TotalColumns;

            //// Assert
            Assert.AreEqual(expectedColumns, act);
        }

        [Test]
        public void TotalColumns_Level2_Return8Columns()
        {
            //// Arrange
            var viewModel = new Lucky3DetailViewModel(details, 2);
            var expectedColumns = 8;

            //// Act
            var act = viewModel.TotalColumns;

            //// Assert
            Assert.AreEqual(expectedColumns, act);
        }
    }
}
