namespace Alpha.Auth.Models
{
    public class Result
    {
        private const int SuccessResultCode = 0;
        private const int UnknownResultcode = -1;
        private const int AccessDeniedCode = 1;

        public static readonly Result Success = new Result(SuccessResultCode, string.Empty);

        public Result()
        {
        }

        public Result(int code, string msg)
        {
            ErrCode = code;
            ErrMsg = msg;
        }

        public string ErrMsg { get; set; }

        public int ErrCode { get; set; }

        public static Result GetAccessDeniedResult(string message)
        {
            return new Result(AccessDeniedCode, message);
        }
    }
}