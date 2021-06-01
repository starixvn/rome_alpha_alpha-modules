namespace Alpha.Web.Filters
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Alpha.Web.Exceptions;
    using Alpha.Web.Helpers;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class PartialUMExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private readonly bool isRedirect;

        public PartialUMExceptionFilter(bool isRedirect = false)
        {
            this.isRedirect = isRedirect;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as PartialUMException;

            if (exception != null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ReturnPartialUMObject(filterContext, exception);
                }
                else
                {
                    GoToPartialUMPage(filterContext, exception);
                }

                filterContext.ExceptionHandled = true;
            }
        }

        private static void ReturnPartialUMObject(ExceptionContext filterContext, PartialUMException exception)
        {
            var response = filterContext.HttpContext.Response;
            response.Clear();
            response.Write("{\"UMFrom\":\"" + exception.UMFrom + "\",\"UMTo\":\"" + exception.UMTo + "\"}");

            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

            response.TrySkipIisCustomErrors = true;
            response.End();
        }

        private void GoToPartialUMPage(ExceptionContext filterContext, PartialUMException exception)
        {
            if (isRedirect)
            {
                filterContext.Result = new RedirectResult(MicroSites.MainUrl($"~/PartialUM/Index?umfrom={exception.UMFrom}&umto={exception.UMTo}"));
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/PartialUM.cshtml",
                    ViewData = new ViewDataDictionary(exception)
                };
            }
        }
    }
}
