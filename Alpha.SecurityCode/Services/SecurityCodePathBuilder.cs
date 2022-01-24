namespace Alpha.SecurityCode.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface SecurityCodePathBuilder
    {
        string BuildResetSecurityCodePath(string rootPath = null, string appVirtualPath = null);

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Reviewed")]
        string BuildVerifyingSecurityCodePath(
            string redirectUrl = "",
            IDictionary<string, string> queryStringItems = null,
            string rootPath = null,
            string appVirtualPath = null);
    }
}