namespace Fanex.Age.UI
{
    public enum ErrorType
    {
        Success = 0,
        Unknown = -1,
        AccessDenied = 1,
        AccountClosed = 2,
        KickedOut = 3,
        UnderMaintenance = 4,
        RequiredSecurityCode = 5,
        NewFlowRequiredSecurityCode = 6,
        WrongSecurityCode = 9009,
        RequireCaptcha = 9010
    }
}