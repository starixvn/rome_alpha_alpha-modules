namespace Alpha.SecurityCode.Web
{
    public interface ISecurityCodeSession
    {
        bool IsAlreadyInputSecurityCode { get; set; }

        string SecurityCode { get; }

        bool IsForcedChangeSecurityCode { get; }

        bool IsInternal { get; }

        string UserSite { get; }
    }
}