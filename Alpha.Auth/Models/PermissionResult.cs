namespace Alpha.Auth.Models
{
    using Alpha.Auth.Configurations;

    public class PermissionResult
    {
        public static readonly PermissionResult DeniedAccessResult = new PermissionResult
        {
            RedirectUrl = AuthorizationSection.Instance.AccessDeniedUrl
        };

        public static readonly PermissionResult AllowedAccessResult = new PermissionResult
        {
            HasPermission = true
        };

        public PermissionResult()
        {
            RedirectUrl = AuthorizationSection.Instance.IndexUrl;
        }

        public bool HasPermission { get; set; }

        public string RedirectUrl { get; set; }
    }
}