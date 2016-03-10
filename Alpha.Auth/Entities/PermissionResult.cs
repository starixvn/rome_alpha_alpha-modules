namespace Alpha.Auth.Entities
{
    using Alpha.Auth.Configurations;

    public class PermissionResult
    {
        public PermissionResult()
        {
            RedirectUrl = AuthorizationSection.Instance.IndexUrl;
        }

        public bool HasPermission { get; set; }

        public string RedirectUrl { get; set; }
    }
}