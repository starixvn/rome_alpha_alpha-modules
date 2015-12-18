namespace Fanex.Age.UI
{
    public class JsonError : JsonData
    {
        /// <summary>
        /// Error code convention:
        /// -1 Unknown error
        /// 0 Success
        /// > 0 Error code, retrieve error message from errMsg field
        /// </summary>
        public const string ErrorCode = "errCode";
        public const string ErrorMsg = "errMsg";

        public JsonError(int errorCode, string errorMsg)
        {
            Add(ErrorCode, errorCode);
            Add(ErrorMsg, errorMsg);
        }

        public JsonError(ErrorType errorType, string errorMsg)
        {
            Add(ErrorCode, (int)errorType);
            Add(ErrorMsg, errorMsg);
        }
    }
}