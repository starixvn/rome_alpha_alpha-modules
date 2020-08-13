namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Models;
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class EnableNetThrottlingAttribute : ActionFilterAttribute
    {
        public ThrottlePolicy Policy { get; set; }
    }
}
