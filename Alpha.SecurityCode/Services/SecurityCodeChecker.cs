namespace Alpha.SecurityCode.Services
{
    public interface SecurityCodeChecker
    {
        bool IsRequiredSecurityCode(bool isBetLists, bool isStatement);

        bool IsRequiredSecurityCode(string relativePath);

        bool IsEnableSecurityCode();

        bool IsForcedChangeSecurityCode();
    }
}