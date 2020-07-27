namespace Alpha.NetThrottling.Mvc
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DisableNetThrottlingAttribute : ActionFilterAttribute
    {
    }
}
