namespace Alpha.Auth.Models
{
    public class BelongingInfo
    {
        public byte RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsCrossLevel(byte uplineRoleId)
            => uplineRoleId - RoleId > 1;
    }
}