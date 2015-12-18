namespace Sunplus.Agent.Model.BL.Bingo
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using Sunplus.Agent.Model.DL.Base;
    using Sunplus.Libraries.ThirdPartyTestData;

    [Obsolete("Should rename to Sunplus.Agent.Model.Bingo")]
    public class BingoAPICaller : IAPICaller
    {
        private const string CookieContainerKey = "BINGOAPICALLER_COOKIECONTAINER";

        public BingoAPICaller()
        {
        }

        private System.Net.CookieContainer CookieContainer
        {
            get
            {
                var session = HttpContext.Current.Session;
                if (session[CookieContainerKey] == null)
                {
                    session[CookieContainerKey] = new System.Net.CookieContainer();
                }

                return session[CookieContainerKey] as System.Net.CookieContainer;
            }
        }        

        #region IAPICaller Members

        public object Invoke(string apiName, List<object> parametersValue)
        {
            LiveDataService service = new LiveDataService();
            service.CookieContainer = this.CookieContainer;
            
            return service.InvokeApi2(apiName, parametersValue.ToArray());
        }

        public bool IsEnable()
        {
            return ConfigurationManager.AppSettings["UseLiveDb"] == "1";
        }
        
        #endregion
    }
}
