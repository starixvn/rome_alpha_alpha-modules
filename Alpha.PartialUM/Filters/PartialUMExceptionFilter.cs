namespace Alpha.PartialUM.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.PartialUM.Exceptions;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class PartialUMExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private readonly bool isRedirect;

        public PartialUMExceptionFilter(bool isRedirect = false)
            => this.isRedirect = isRedirect;

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
                filterContext.Result = new RedirectResult($"{MainPath}PartialUM/Index?umfrom={exception.UMFrom}&umto={exception.UMTo}");
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

        private string MainPath
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session.IsCookieless)
                {
                    var currentSession = UserSession.Current;
                    var mainSessionId = currentSession.GetProperty<string>("MainSessionID");

                    if (!string.IsNullOrWhiteSpace(mainSessionId))
                    {
                        return $"{currentSession.GetProperty<string>("MainDomain")}/(S({mainSessionId}))/site-main/";
                    }
                }

                return MicroSites.MainUrl();
            }
        }
    }
}
