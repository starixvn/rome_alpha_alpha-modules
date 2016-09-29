namespace Alpha.SecurityCode.Web
{
    public interface SecurityCodeHttpContext
    {
        bool IsValidRequest();

        string GetRelativePath();

        bool IsAjaxRequest();

        void DenyAjaxRequest();

        void RedirectToVerifySecurityCodePage();
    }
}