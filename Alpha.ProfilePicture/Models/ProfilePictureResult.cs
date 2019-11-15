namespace Alpha.AccountInfo.Features.Shared.Models
{
    using Alpha.ProfilePicture.AppResources;
    using System.Net;

    public class ProfilePictureResult
    {
        public ProfilePictureResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

        public static ProfilePictureResult CreateSuccessResult(string message = "")
            => new ProfilePictureResult(0, message);

        public static ProfilePictureResult CreateErrorResult(int code, string message)
            => new ProfilePictureResult(code, message);

        public static ProfilePictureResult CreateForbiddenResult()
            => new ProfilePictureResult((int)HttpStatusCode.Forbidden, ProfilePictureResources.NotAuthorized);

        public bool IsSuccess => Code.Equals(0);
    }
}
