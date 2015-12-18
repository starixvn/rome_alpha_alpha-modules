namespace Fanex.Razorbill.Module.RnGCasino
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.Razorbill.Module.RnGCasino.App_Start;

    /// <summary>
    /// Company Registration.
    /// </summary>
    public class CompanyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get 
            { 
                return "Company"; 
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {     
            RegisterRoutes(context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_Default",
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new { controller = "ContactUs", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.Razorbill.Module.Company.Controllers" });
        }
    }
}
