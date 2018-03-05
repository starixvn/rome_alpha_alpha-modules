namespace Sunplus.Agent.Model.DL.Base
{
    using System;
    using System.Data;
    using Fanex.Data.DNAide;
    using System.Configuration;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ServiceSoap", Namespace = "http://tempuri.org/")]
    [Obsolete("Please rename to Sunplus.Agent.Model.DataAccessLibrary")]
    public partial class LiveDataService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        /// <remarks/>
        public LiveDataService()
        {
            this.Url = ConfigurationManager.AppSettings["LiveDataService"];
        }

        #region Invoke

        private System.Threading.SendOrPostCallback invokeOperationCompleted;

        /// <remarks/>
        public event InvokeCompletedEventHandler InvokeCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Invoke", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DbResult Invoke(string storeProcedure, string[] parametersName, object[] parametersValue, string dbaType)
        {
            object[] results = this.Invoke("Invoke", new object[] { storeProcedure, parametersName, parametersValue, dbaType });
            return (DbResult)results[0];
        }

        /// <remarks/>
        public System.IAsyncResult BeginInvoke(string storeProcedure, string[] parametersName, object[] parametersValue, string dbaType, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Invoke", new object[] { storeProcedure, parametersName, parametersValue, dbaType }, callback, asyncState);
        }

        /// <remarks/>
        public new DbResult EndInvoke(System.IAsyncResult asyncResult)
        {
            object[] results = base.EndInvoke(asyncResult);
            return (DbResult)results[0];
        }

        /// <remarks/>
        public void InvokeAsync(string storeProcedure, string[] parametersName, object[] parametersValue, string dbaType)
        {
            this.InvokeAsync(storeProcedure, parametersName, parametersValue, dbaType, null);
        }

        /// <remarks/>
        public void InvokeAsync(string storeProcedure, string[] parametersName, object[] parametersValue, string dbaType, object userState)
        {
            if (this.invokeOperationCompleted == null)
            {
                this.invokeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInvokeOperationCompleted);
            }

            this.InvokeAsync("Invoke", new object[] { storeProcedure, parametersName, parametersValue, dbaType }, this.invokeOperationCompleted, userState);
        }

        private void OnInvokeOperationCompleted(object arg)
        {
            if (this.InvokeCompleted != null)
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = (System.Web.Services.Protocols.InvokeCompletedEventArgs)arg;
                this.InvokeCompleted(this, new InvokeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        #endregion

        #region InvokeApi

        private System.Threading.SendOrPostCallback invokeApiOperationCompleted;

        /// <remarks/>
        public event InvokeApiCompletedEventHandler InvokeApiCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InvokeApi", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DataTable[] InvokeApi(string apiName, object[] parametersValue)
        {
            object[] results = this.Invoke("InvokeApi", new object[] { apiName, parametersValue });
            return (DataTable[])results[0];
        }

        /// <remarks/>
        public System.IAsyncResult BeginInvokeApi(string apiName, object[] parametersValue, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InvokeApi", new object[] { apiName, parametersValue }, callback, asyncState);
        }

        /// <remarks/>
        public object EndInvokeApi(System.IAsyncResult asyncResult)
        {
            object[] results = base.EndInvoke(asyncResult);
            return (object)results[0];
        }

        /// <remarks/>
        public void InvokeApiAsync(string apiName, object[] parametersValue)
        {
            this.InvokeApiAsync(apiName, parametersValue, null);
        }

        /// <remarks/>
        public void InvokeApiAsync(string apiName, object[] parametersValue, object userState)
        {
            if (this.invokeApiOperationCompleted == null)
            {
                this.invokeApiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInvokeApiOperationCompleted);
            }

            this.InvokeAsync("InvokeApi", new object[] { apiName, parametersValue }, this.invokeApiOperationCompleted, userState);
        }

        private void OnInvokeApiOperationCompleted(object arg)
        {
            if (this.InvokeApiCompleted != null)
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = (System.Web.Services.Protocols.InvokeCompletedEventArgs)arg;
                this.InvokeApiCompleted(this, new InvokeApiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        #endregion

        #region InvokeApi2

        private System.Threading.SendOrPostCallback invokeApi2OperationCompleted;

        /// <remarks/>
        public event InvokeApi2CompletedEventHandler InvokeApi2Completed;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InvokeApi2", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]

        public DataTable[] InvokeApi2(string apiName, object[] parametersValue)
        {
            object[] results = this.Invoke("InvokeApi2", new object[] { apiName, parametersValue });
            return (DataTable[])results[0];
        }

        /// <remarks/>
        public System.IAsyncResult BeginInvokeApi2(string apiName, object[] parametersValue, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InvokeApi2", new object[] { apiName, parametersValue }, callback, asyncState);
        }

        /// <remarks/>
        public object EndInvokeApi2(System.IAsyncResult asyncResult)
        {
            object[] results = base.EndInvoke(asyncResult);
            return (object)results[0];
        }

        /// <remarks/>
        public void InvokeApi2Async(string apiName, object[] parametersValue)
        {
            this.InvokeApi2Async(apiName, parametersValue, null);
        }

        /// <remarks/>
        public void InvokeApi2Async(string apiName, object[] parametersValue, object userState)
        {
            if (this.invokeApi2OperationCompleted == null)
            {
                this.invokeApi2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnInvokeApi2OperationCompleted);
            }

            this.InvokeAsync("InvokeApi2", new object[] { apiName, parametersValue }, this.invokeApi2OperationCompleted, userState);
        }

        private void OnInvokeApi2OperationCompleted(object arg)
        {
            if (this.InvokeApi2Completed != null)
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = (System.Web.Services.Protocols.InvokeCompletedEventArgs)arg;
                this.InvokeApi2Completed(this, new InvokeApi2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        #endregion

        #region InvokeRemoteRef

        private System.Threading.SendOrPostCallback invokeRemoteRefOperationCompleted;

        /// <remarks/>
        public event InvokeRemoteRefCompletedEventHandler InvokeRemoteRefCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InvokeRemoteRef", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InvokeRemoteRef(string functionName, object[] parameters)
        {
            object[] results = this.Invoke("InvokeRemoteRef", new object[] { functionName, parameters });
            return (string)results[0];
        }

        /// <remarks/>
        public System.IAsyncResult BeginInvokeRemoteRef(string functionName, object[] parameters, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InvokeRemoteRef", new object[] { functionName, parameters }, callback, asyncState);
        }

        /// <remarks/>
        public object EndInvokeRemoteRef(System.IAsyncResult asyncResult)
        {
            object[] results = base.EndInvoke(asyncResult);
            return (object)results[0];
        }

        /// <remarks/>
        public void InvokeRemoteRefAsync(string functionName, object[] parameters)
        {
            this.InvokeRemoteRefAsync(functionName, parameters, null);
        }

        /// <remarks/>
        public void InvokeRemoteRefAsync(string functionName, object[] parameters, object userState)
        {
            if (this.invokeRemoteRefOperationCompleted == null)
            {
                this.invokeRemoteRefOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInvokeRemoteRefOperationCompleted);
            }

            this.InvokeAsync("InvokeRemoteRef", new object[] { functionName, parameters }, this.invokeRemoteRefOperationCompleted, userState);
        }

        private void OnInvokeRemoteRefOperationCompleted(object arg)
        {
            if (this.InvokeRemoteRefCompleted != null)
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = (System.Web.Services.Protocols.InvokeCompletedEventArgs)arg;
                this.InvokeRemoteRefCompleted(this, new InvokeRemoteRefCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        #endregion

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    #region Invoke

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void InvokeCompletedEventHandler(object sender, InvokeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InvokeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal InvokeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DbResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return (DbResult)this.results[0];
            }
        }
    }

    #endregion

    #region InvokeApi

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void InvokeApiCompletedEventHandler(object sender, InvokeApiCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InvokeApiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal InvokeApiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return (object)this.results[0];
            }
        }
    }

    #endregion

    #region InvokeApi2

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void InvokeApi2CompletedEventHandler(object sender, InvokeApi2CompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InvokeApi2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal InvokeApi2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return (object)this.results[0];
            }
        }
    }

    #endregion

    #region InvokeRemoteRef

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void InvokeRemoteRefCompletedEventHandler(object sender, InvokeRemoteRefCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InvokeRemoteRefCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal InvokeRemoteRefCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return (object)this.results[0];
            }
        }
    }

    #endregion
}