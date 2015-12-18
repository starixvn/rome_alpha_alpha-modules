namespace Sunplus.Agent.Model.BL.Casino
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using Sunplus.Agent.Model.DL.Base;
    using Sunplus.Libraries.ThirdPartyTestData;

    public class CasinoAPICaller : IAPICaller
    {
        private const string CookieContainerKey = "CASINOAPICALLER_COOKIECONTAINER";

        public CasinoAPICaller()
        {
        }

        private System.Net.CookieContainer CookieContainer
        {
            get
            {
                if (HttpContext.Current.Session[CookieContainerKey] == null)
                {
                    HttpContext.Current.Session[CookieContainerKey] = new System.Net.CookieContainer();
                }

                return HttpContext.Current.Session[CookieContainerKey] as System.Net.CookieContainer;
            }
        }

        #region IAPICaller Members

        public object Invoke(string apiName, List<object> parametersValue)
        {
            var service = new LiveDataService { CookieContainer = this.CookieContainer };
            return service.InvokeApi(apiName, parametersValue.ToArray());
        }

        public bool IsEnable()
        {
            return ConfigurationManager.AppSettings["UseLiveDb"] == "1";
        }

        #endregion IAPICaller Members
    }
}