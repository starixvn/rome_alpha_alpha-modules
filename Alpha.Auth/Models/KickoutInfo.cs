namespace Alpha.Auth.Models
{
    using System;

    [Serializable]
    public class KickoutInfo
    {
        public KickoutInfo(bool isKickedOut, byte kickOutReason = 0)
        {
            IsKickedOut = isKickedOut;
            KickOutReason = kickOutReason;
        }

        public bool IsKickedOut { get; }

        public byte KickOutReason { get; }
    }
}
