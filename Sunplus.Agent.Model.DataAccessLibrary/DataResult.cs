namespace Sunplus.Agent.Model.DL.Base
{
    using System;

    [Serializable]
    [Obsolete("Please rename to Sunplus.Agent.Model.DataAccessLibrary")]
    public class DataResult
    {
        public DataResult()
        {
        }

        public DataResult(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
