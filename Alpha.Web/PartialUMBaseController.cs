namespace Alpha.Web
{
    using System.Web.Mvc;
    using Alpha.Web.Exceptions;

    public class PartialUMBaseController : BaseController
    {
        private readonly string partialUMViewName;

        protected PartialUMBaseController(string partialUMViewName = null)
        {
            this.partialUMViewName = partialUMViewName ?? "~/Views/Shared/PartialUM.cshtml";
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var partialUMException = filterContext.Exception as PartialUMException;

            if (partialUMException != null)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = partialUMViewName,
                    ViewData = new ViewDataDictionary(partialUMException)
                };

                filterContext.ExceptionHandled = true;
            }
        }
    }
}
