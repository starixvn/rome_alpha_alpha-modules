namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Web;

    public class CheckBelongingHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(AcquireRequestState);
        }

        private void AcquireRequestState(object sender, EventArgs e)
        {
        }

        #endregion IHttpModule Members
    }
}