namespace Fanex.Age.UI
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Web;
    using Fanex.Security.SpamFilter;
    using Sunplus.Agent.Model.Entities;

    public abstract class TplHandlerSpamFilter : TplHandler
    {
        private static ConcurrentDictionary<int, object> _spamFiltersLockObjects = new ConcurrentDictionary<int, object>();

        public override void ProcessRequest(HttpContext context)
        {
            _context = context;

            var lockObject = _spamFiltersLockObjects.GetOrAdd(UserSession.Current.CustId, new object());

            lock (lockObject)
            {
                var spamFiler = (from blocker in HttpContext.Current.GetSpamFilterBlockers()
                                 select blocker.Name).ToList();

                if (spamFiler.Count > 0)
                {
                    context.Response.ContentType = "text/plain";
                    var error = new Result(ErrorType.RequireCaptcha, "This action is invalid, please contact your administrator!");
                    _Result.AsResult(error);
                    Response.Write(Json.Render(_Result));

                    return;
                }

                base.ProcessRequest(context);

                var result = new Result();
                if (!result.Equals(_View))
                {
                    return;
                }

                result = _View as Result;

                if (result != null
                    && (result.ErrCode.Equals(0)
                        || result.ErrCode.Equals(999)))
                {
                    HttpContext.Current.SpamFilterIncrement();
                }
            }
        }
    }
}