namespace Alpha.SubAccount.Tests.Models
{
    using System.Runtime.Serialization;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class PageAccessTests
    {
        [Test]
        public void PageAccess_Always_AllPermissionsIsFalse()
        {
            // Act
            var actualPageAccess = new PageAccess();

            // Assert
            Assert.False(
                actualPageAccess.CreateAccount ||
                actualPageAccess.UpdateAccount ||
                actualPageAccess.Transfer ||
                actualPageAccess.ViewTotalBets ||
                actualPageAccess.ViewReports ||
                actualPageAccess.ViewLog ||
                actualPageAccess.ViewMemberInfo ||
                actualPageAccess.GetPermission("[L]"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectCreateAccountPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.CreateAccount);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectUpdateAccountPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.UpdateAccount);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectTransferPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.Transfer);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectViewTotalBetsPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.ViewTotalBets);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectViewReportsPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.ViewReports);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectViewLogPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.ViewLog);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectViewMemberInfoPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.ViewMemberInfo);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageAccessWithSerializationInfo_Always_SetCorrectViewBetListsPermission(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);

            // Act
            var actualPageAccess = new PageAccess(info, context);

            // Assert
            Assert.AreEqual(permission, actualPageAccess.GetPermission("[L]"));
        }

        [TestCase("[A]", true)]
        [TestCase("[A]", false)]
        [TestCase("[B]", true)]
        [TestCase("[B]", false)]
        public void SetPermission_Always_SetCorrectValue(string key, bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.SetPermission(key, permission);

            // Assert
            Assert.AreEqual(permission, pageAccess.GetPermission(key));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetPermission_Always_ReturnCorrectValue(bool permission)
        {
            // Arrange
            StreamingContext context = new StreamingContext();
            var info = CreateSerializationInfo(permission);
            var pageAccess = new PageAccess(info, context);

            // Act
            var actualPermission = pageAccess.GetPermission("[A]");

            // Assert
            Assert.AreEqual(permission, actualPermission);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        public void PageAccessWithString_IsNullOrEmpty_ReturnDefaultPageAccess(string permissions)
        {
            // Act
            var actualPageAccess = (PageAccess)permissions;

            // Assert
            Assert.False(
               actualPageAccess.CreateAccount ||
               actualPageAccess.UpdateAccount ||
               actualPageAccess.Transfer ||
               actualPageAccess.ViewTotalBets ||
               actualPageAccess.ViewReports ||
               actualPageAccess.ViewLog ||
               actualPageAccess.ViewMemberInfo ||
               actualPageAccess.GetPermission("[L]"));
        }

        [Test]
        public void PageAccessWithString_NotNullOrEmpty_ReturnCorrectPageAccess()
        {
            // Act
            var actualPageAccess = (PageAccess)"[A][C][D][G][L]";

            // Assert
            Assert.True(
               actualPageAccess.CreateAccount &&
               !actualPageAccess.UpdateAccount &&
               actualPageAccess.Transfer &&
               actualPageAccess.ViewTotalBets &&
               !actualPageAccess.ViewReports &&
               actualPageAccess.ViewLog &&
               !actualPageAccess.ViewMemberInfo &&
               actualPageAccess.GetPermission("[L]"));
        }

        [Test]
        public void ConvertToString_EmptyPageAccess_ReturnEmptyString()
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            var actualPageAccess = (string)pageAccess;

            // Assert
            Assert.IsEmpty(actualPageAccess);
        }

        [Test]
        public void ConvertToString_HasPermission_ReturnCorrectString()
        {
            // Arrange
            var pageAccess = new PageAccess();
            pageAccess.SetPermission("[A]", true);
            pageAccess.SetPermission("[C]", true);
            pageAccess.SetPermission("[D]", true);
            pageAccess.SetPermission("[G]", true);
            pageAccess.SetPermission("[L]", true);

            var expectedPageAccess = "[A][C][D][G][L]";

            // Act
            var actualPageAccess = (string)pageAccess;

            // Assert
            Assert.AreEqual(expectedPageAccess, actualPageAccess);
        }

        [Test]
        public void IsEmptyPermission_DefaultPageAccess_ReturnsTrue()
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            var actualIsEmptyPermission = pageAccess.IsEmptyPermission();

            // Assert
            Assert.True(actualIsEmptyPermission);
        }

        [Test]
        public void IsEmptyPermission_HasPermission_ReturnsFalse()
        {
            // Arrange
            var pageAccess = new PageAccess();
            pageAccess.SetPermission("[C]", true);
            pageAccess.SetPermission("[D]", true);

            // Act
            var actualIsEmptyPermission = pageAccess.IsEmptyPermission();

            // Assert
            Assert.False(actualIsEmptyPermission);
        }

        [Test]
        public void SetFullPermission_Always_SetAllPermissionToTrue()
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.SetFullPermission();

            // Assert
            Assert.True(
                pageAccess.CreateAccount &&
                pageAccess.UpdateAccount &&
                pageAccess.Transfer &&
                pageAccess.ViewTotalBets &&
                pageAccess.ViewReports &&
                pageAccess.ViewLog &&
                pageAccess.ViewMemberInfo &&
                pageAccess.GetPermission("[L]"));
        }

        [Test]
        public void Contains_NotContainComparedValue_ReturnFalse()
        {
            // Arrange
            var pageAccess = (PageAccess)"[A][C][D]";
            var comparedPageAccess = (PageAccess)"[A][B][D]";

            // Act
            var actualContains = comparedPageAccess.Contains(pageAccess);

            // Assert
            Assert.IsFalse(actualContains);
        }

        [Test]
        public void Contains_ContainComparedValue_ReturnTrue()
        {
            // Arrange
            var pageAccess = (PageAccess)"[A][C][D]";
            var comparedPageAccess = (PageAccess)"[A][C][D]";

            // Act
            var actualContains = comparedPageAccess.Contains(pageAccess);

            // Assert
            Assert.IsTrue(actualContains);
        }

        [Test]
        public void GetObjectData_Always_SetCorrectPermissionToInfo()
        {
            // Arrange
            var pageAccess = (PageAccess)"[A][C][D]";
            StreamingContext context = new StreamingContext();
            var info = new SerializationInfo(typeof(PageAccess), new FormatterConverter());

            // Act
            pageAccess.GetObjectData(info, context);

            // Assert
            Assert.IsTrue(
                info.GetBoolean("[A]") == true &&
                info.GetBoolean("[B]") == false &&
                info.GetBoolean("[C]") == true &&
                info.GetBoolean("[D]") == true &&
                info.GetBoolean("[E]") == false &&
                info.GetBoolean("[G]") == false &&
                info.GetBoolean("[H]") == false &&
                info.GetBoolean("[L]") == false);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetCreateAccount_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.CreateAccount = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.CreateAccount);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetUpdateAccount_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.UpdateAccount = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.UpdateAccount);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetTransfer_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.Transfer = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.Transfer);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetViewTotalBets_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.ViewTotalBets = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.ViewTotalBets);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetViewReports_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.ViewReports = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.ViewReports);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetViewLog_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.ViewLog = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.ViewLog);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetViewMemberInfo_Always_SetCorrectValue(bool permission)
        {
            // Arrange
            var pageAccess = new PageAccess();

            // Act
            pageAccess.ViewMemberInfo = permission;

            // Assert
            Assert.AreEqual(permission, pageAccess.ViewMemberInfo);
        }

        private SerializationInfo CreateSerializationInfo(bool permission)
        {
            var info = new SerializationInfo(typeof(PageAccess), new FormatterConverter());
            info.AddValue("[A]", permission);
            info.AddValue("[B]", permission);
            info.AddValue("[C]", permission);
            info.AddValue("[D]", permission);
            info.AddValue("[E]", permission);
            info.AddValue("[G]", permission);
            info.AddValue("[H]", permission);
            info.AddValue("[L]", permission);

            return info;
        }
    }
}
