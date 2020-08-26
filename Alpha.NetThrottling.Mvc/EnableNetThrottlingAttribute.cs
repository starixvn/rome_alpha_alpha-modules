namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Models;
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class EnableNetThrottlingAttribute : ActionFilterAttribute
    {
        public IThrottlePolicy Policy { get; set; }
    }
}
