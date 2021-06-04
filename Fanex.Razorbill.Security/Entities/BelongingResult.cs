namespace Fanex.Razorbill.Security.Entities
{
    public class BelongingResult
    {
        public BelongingResult(int roleId, bool isActive)
        {
            RoleId = roleId;
            IsActive = isActive;
        }

        public int RoleId { get; }

        public bool IsActive { get; }
    }
}