namespace Fanex.Razorbill.Security
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ModuleRegistrator : AreaRegistration
    {
        private IList<string> _controllers = new List<string>();

        public override string AreaName
        {
            get
            {
                return "Security";
            }
        }

        /*protected string ModuleName
        {
            get
            {
                return "SecurityCode";
            }
        }*/

        protected string ModuleNameSpace
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name + ".Controllers";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            if (!string.IsNullOrWhiteSpace(this.ModuleNameSpace))
            {
                context.MapRoute(
                    AreaName,
                    AreaName + "/{controller}/{action}/{id}",
                    new { action = "Index", id = UrlParameter.Optional },
                    new[] { ModuleNameSpace });
            }

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void RegisterController(string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                return;
            }

            _controllers.Add(controllerName);
        }
    }
}