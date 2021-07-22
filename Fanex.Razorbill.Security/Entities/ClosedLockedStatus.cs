namespace Fanex.Razorbill.Security.Entities
{
    public class ClosedLockedStatus
    {
        public ClosedLockedStatus(bool isActive = true, byte locked = 0)
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
