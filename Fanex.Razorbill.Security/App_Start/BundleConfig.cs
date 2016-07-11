namespace Fanex.Razorbill.Security
{
    using System.Web.Optimization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/App_Themes/Apo/styles/securitycode-css").Include(
                "~/App_Themes/Apo/Security/howtouse.css",
                "~/App_Themes/Apo/Security/securityCode.css"));

            bundles.Add(new ScriptBundle("~/bundles/forcechangesecuritycode-js").Include(
                "~/Areas/Security/Views/Scripts/Sha1.js",
                "~/Areas/Security/Views/Scripts/Sha256.js",
                "~/Areas/Security/Views/Scripts/core.js",
                "~/Areas/Security/Views/Scripts/forcechangesecuritycode.js"));

            bundles.Add(new ScriptBundle("~/bundles/securitycodewithoutvalidation-js").Include(
                "~/Areas/Security/Views/Scripts/core.js",
                "~/Areas/Security/Views/Scripts/forcechangesecuritycodewithoutvalidation.js"));

            bundles.Add(new ScriptBundle("~/bundles/verifysecuritycode-js").Include(
                "~/Areas/Security/Views/Scripts/Sha1.js",
                "~/Areas/Security/Views/Scripts/Sha256.js",
                "~/Areas/Security/Views/Scripts/core.js",
                "~/Areas/Security/Views/Scripts/verifysecuritycode.js"));

            bundles.Add(new ScriptBundle("~/bundles/changesecuritycode-js").Include(
                "~/Areas/Security/Views/Scripts/Sha1.js",
                "~/Areas/Security/Views/Scripts/Sha256.js",
                "~/Areas/Security/Views/Scripts/core.js",
                "~/Areas/Security/Views/Scripts/changesecuritycode.js"));

            bundles.Add(new ScriptBundle("~/bundles/resetsecuritycode-js").Include(
                "~/Areas/Security/Views/Scripts/core.js",
                "~/Areas/Security/Views/Scripts/resetsecuritycode.js"));
        }
    }
}