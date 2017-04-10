namespace Alpha.SmartPunter.Tests.Services
{
    using DataAccesses;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using SmartPunter.Criteria;
    using SmartPunter.Services;

    [TestFixture]
    public class SmartPunterServiceTests
    {
        private ISmartPunterDataAccess _smartPunterDataAccess;
        private SmartPunterService _smartPunterService;

        [SetUp]
        public void SetUp()
        {
            _smartPunterDataAccess = Substitute.For<ISmartPunterDataAccess>();
            _smartPunterService = new SmartPunterService(_smartPunterDataAccess);
        }

        #region CheckInternalAdmin

        [TestCase(int.MinValue, false)]
        [TestCase(-100, false)]
        [TestCase(0, false)]
        [TestCase(8, false)]
        [TestCase(1176070, true)]
        public void CheckInternalAdmin_ReturnNoneInternalAdmin(int customerId, bool expectedResult)
        {
            // Arrange.
            _smartPunterDataAccess.CheckInternalAdmin(Arg.Is<InternalAdminChecker>(checker => checker.CustId == customerId))
                .Returns(expectedResult);

            // Act.
            var actualResult = _smartPunterService.CheckInternalAdmin(customerId);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult);
        }

        #endregion CheckInternalAdmin

        #region CheckSmartPunter

        [TestCase(int.MinValue)]
        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(1176070)]
        public void CheckSmartPunter_ReturnNoneSmartPunter(int customerId)
        {
            // Arrange.
            var expectedResult = new SMACustomer();
            _smartPunterDataAccess.CheckSmartPunter(Arg.Is<SmartPunterSMAChecker>(checker => checker.CustId == customerId))
                .Returns(expectedResult);

            // Act.
            var actualResult = _smartPunterService.CheckSmartPunter(customerId);

            // Assert.
            Assert.IsTrue(
                expectedResult.IsSmartPunter == actualResult.IsSmartPunter &&
                expectedResult.HasDownlineSmartPunter == actualResult.HasDownlineSmartPunter);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void CheckSmartPunter_ReturnSmartPunter(bool isSmartPunter, bool hasDownlineSmartPunter)
        {
            // Arrange.
            int customerId = 1176070;
            var expectedResult = new SMACustomer(isSmartPunter, hasDownlineSmartPunter);
            _smartPunterDataAccess.CheckSmartPunter(Arg.Is<SmartPunterSMAChecker>(checker => checker.CustId == customerId))
                .Returns(expectedResult);

            // Act.
            var actualResult = _smartPunterService.CheckSmartPunter(customerId);

            // Assert.
            Assert.IsTrue(
                expectedResult.IsSmartPunter == actualResult.IsSmartPunter &&
                expectedResult.HasDownlineSmartPunter == actualResult.HasDownlineSmartPunter);
        }

        #endregion CheckSmartPunter

        #region WriteLog

        [Test]
        public void WriteLog_CallWriteLog_FromDataAccess()
        {
            // Arrange.
            const int LogActionId = 31008;
            string userNames = "member01,member011,member012";
            bool isRestricted = false;
            int userId = 8;
            string ip = "127.0.0.1";

            // Act.
            _smartPunterService.WriteLog(userNames, isRestricted, userId, ip);

            // Assert.
            _smartPunterDataAccess
                .Received()
                .WriteLog(Arg.Is<SmartPunterWriteLog>(checker =>
                    checker.ListUserName == userNames &&
                    checker.ActionId == LogActionId &&
                    checker.IsRestricted == isRestricted &&
                    checker.UserId == userId &&
                    checker.IP == ip));
        }

        #endregion WriteLog

        #region CheckSmartPunterAndWriteLog

        [Test]
        public void CheckSmartPunterAndWriteLog_CallCheckSmartPunterAndWriteLog_FromDataAccess()
        {
            // Arrange.
            const int LogActionId = 31008;
            string userNames = "member01,member011,member012";
            bool isRestricted = false;
            int userId = 8;
            string ip = "127.0.0.1";

            // Act.
            _smartPunterService.CheckSmartPunterAndWriteLog(userNames, isRestricted, userId, ip);

            // Assert.
            _smartPunterDataAccess
                .Received()
                .CheckSmartPunterAndWriteLog(Arg.Is<SmartPunterSMACheckerWriteLog>(checker =>
                    checker.ListUserName == userNames &&
                    checker.ActionId == LogActionId &&
                    checker.IsRestricted == isRestricted &&
                    checker.UserId == userId &&
                    checker.IP == ip));
        }

        #endregion CheckSmartPunterAndWriteLog

        #region WriteLogNoData

        [TestCase(1176070)]
        [TestCase(null)]
        public void WriteLogNoData_CallWriteLogNoData_FromDataAccess(int? expectedResult)
        {
            // Arrange.
            const int LogActionId = 31008;
            bool isRestricted = false;
            int userId = 8;
            string ip = "127.0.0.1";

            // Act.
            _smartPunterService.WriteLogNoData(expectedResult, isRestricted, userId, ip);

            // Assert.
            _smartPunterDataAccess
                .Received()
                .WriteLogNoData(Arg.Is<SmartPunterWriteLogNoData>(checker =>
                    checker.CustId == expectedResult &&
                    checker.ActionId == LogActionId &&
                    checker.IsRestricted == isRestricted &&
                    checker.UserId == userId &&
                    checker.IP == ip));
        }

        #endregion WriteLogNoData

        #region HasSmartPunterPermission

        [TestCase(int.MinValue, false)]
        [TestCase(-100, false)]
        [TestCase(0, false)]
        [TestCase(1176070, false)]
        [TestCase(1176070, true)]
        public void HasSmartPunterPermission_AllTestCases(int loggedInUserId, bool expectedResult)
        {
            // Arrange.
            const string SmartPunterFunctionName = "[SmartPunter]";
            _smartPunterDataAccess.CheckPermission(Arg.Is<NAPUserPermission>(checker => checker.CustId == loggedInUserId), SmartPunterFunctionName)
                .Returns(expectedResult);

            // Act.
            var actualResult = _smartPunterService.HasSmartPunterPermission(loggedInUserId);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult);
        }

        #endregion HasSmartPunterPermission
    }
}