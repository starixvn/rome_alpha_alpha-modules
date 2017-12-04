using System.Threading;
using RemotingRef;

namespace Fanex.Products.Utilities
{
    internal static class RefDataUtility
    {
        private static string CurrentTWLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "en-US":
                        return "en";

                    case "zh-TW":
                        return "ch"; // Traditional

                    case "zh-CN":
                        return "cs"; // Simplified

                    case "ja-JP":
                        return "jp";

                    case "th-TH":
                        return "th";

                    case "ko-KR":
                        return "ko";

                    case "vi-VN":
                        return "vn";

                    default:
                        return "en";
                }
            }
        }

        public static string GetProductName(int productId)
        {
            return RefData.GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), CurrentTWLanguage, string.Empty);
        }
    }
}
