namespace Alpha.SecurityCode.Web
{
    public interface SecurityCodeSession
    {
        bool IsAlreadyInputSecurityCode { get; set; }

        string SecurityCode { get; }

        bool IsForcedChangeSecurityCode { get; }

        bool IsInternal { get; }

        string UserSite { get; }

        bool IsEnabledOtp { get; set; }

        bool IsEnabledSecurityCodeInBetLists { get; }

        bool IsEnabledSecurityCodeInStatement { get; }

        string MainDomain { get; }
    }
}