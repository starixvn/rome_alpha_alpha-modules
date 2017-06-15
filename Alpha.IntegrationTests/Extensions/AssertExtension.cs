namespace Alpha.IntegrationTests.Extensions
{
    using System;
    using Xunit;
    using Xunit.Sdk;

    public static class AssertExtension
    {
        public static void Equal(string expected, string actual)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch (XunitException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
