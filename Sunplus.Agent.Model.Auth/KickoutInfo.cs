namespace Sunplus.Agent.Model.Auth
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

        public string KickOutReasonResourceKey
        {
            get
            {
                const byte ResetPasswordForceKickout = 1;
                const byte ProtectedModeForceKickout = 2;

                switch (KickOutReason)
                {
                    case ResetPasswordForceKickout:
                        return "KickoutByForceChangePassword";

                    case ProtectedModeForceKickout:
                        return "KickoutByProtectedMode";

                    default:
                        return "DuplicationLogin";
                }
            }
        }
    }
}
