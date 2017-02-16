namespace Alpha.Web.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Web.Helpers;
    using Xunit;

    public class AppPathHelperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetAppPath_WithEmptyAppName_ThrowsArgumentException(string appName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AppPathHelper.GetAppPath(appName, string.Empty));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetAppPath_WithEmptyVirtualPath_ThrowsArgumentException(string virtualPath)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AppPathHelper.GetAppPath(string.Empty, virtualPath));
        }

        [Fact]
        public void GetAppPath_WithAppNameAndVirtualPathAndNotHasContext_ReturnsEmpty()
        {
            // Arrange
            var validAppName = "ValidAppName";
            var validVirtualPath = "ValidVirtualPath";

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, validVirtualPath);

            // Assert
            Assert.Empty(actualAppPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetRelativePath_NullOrWhiteSpaceVirtualPath_ReturnsVirtualPath(string virtualPath)
        {
            // Arrange
            var expectedPath = virtualPath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, "AbsolutePath");

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetRelativePath_NullOrWhiteSpaceAbsolutePath_ReturnsVirtualPath(string absolutePath)
        {
            // Arrange
            var virtualPath = "VirtualPath";
            var expectedPath = virtualPath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData("VirtualPath/", "")]
        [InlineData("VirtualPathA", "")]
        [InlineData("VirtualPath/SubPath", "SubPath")]
        [InlineData("VirtualPath/VirtualPath/SubPath", "VirtualPath/SubPath")]
        public void GetRelativePath_AbsolutePathStartWithVirtualPath_ReturnsCorrectRelativePath(
            string absolutePath,
            string expectedPath)
        {
            // Arrange
            var virtualPath = "VirtualPath";

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData("VirtualPath2/")]
        [InlineData("VirtualPathA")]
        [InlineData("VirtualPath2/SubPath")]
        [InlineData("VirtualPath2/VirtualPath/SubPath")]
        public void GetRelativePath_AbsolutePathNotStartWithVirtualPath_ReturnsCorrectRelativePath(string absolutePath)
        {
            // Arrange
            var virtualPath = "VirtualPath1";
            var expectedPath = absolutePath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }
    }
}
