namespace Alpha.SecurityCode.Services
{
    public interface SecurityCodeChecker
    {
        bool IsRequiredSecurityCode(string relativePath);

        bool IsEnableSecurityCode();

        bool IsForcedChangeSecurityCode();
    }
}