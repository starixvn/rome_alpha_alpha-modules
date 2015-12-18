namespace Fanex.Razorbill.Module.RnGCasino.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    internal static class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterContactUsBundles(bundles);
        }

        private static void RegisterContactUsBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Areas/Company/Views/ContactUs/Styles/ContactUsStyles").Include(
                "~/Areas/Company/Views/ContactUs/Styles/styles.css"));
        }
    }
}
