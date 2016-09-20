namespace Sunplus.Agent.Model.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using CasinoLibrary.SBAgent;
    using Sunplus.Agent.Model.BL.Casino;

    public class CasinoServiceImpl : CasinoService
    {
        private readonly CasinoBO casinoBO;

        public CasinoServiceImpl()
        {
            if(HttpContext.Current != null
                && HttpContext.Current.Session != null)
            {
                casinoBO = new CasinoBO(HttpContext.Current.Session.SessionID);
            }
        }

        public Customer Customer
        {
            get
            {
                return casinoBO.Customer;
            }
        }

        public Report Report
        {
            get
            {
                return casinoBO.Report;
            }
        }

        public string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }
    }
}
