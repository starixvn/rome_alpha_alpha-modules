using System.Threading;
using Fanex.RemotingRef;

namespace Fanex.Products.Utilities
{
    internal static class RefDataUtility
    {
        public static string GetProductName(int productId)
            => Instance.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty);

        private static IRemotingRefService Instance
            => new RemotingRefService();

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
