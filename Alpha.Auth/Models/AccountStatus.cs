namespace Alpha.Auth.Models
{
    public class AccountStatus
    {
        public AccountStatus(bool isActive, byte locked)
        {
            IsActive = isActive;
            Locked = locked;
        }

        public bool IsActive { get; }

        public bool IsClosed => !IsActive;

        public byte Locked { get; }

        public bool IsLocked => Locked != 0;
    }
}
