namespace Alpha.NetThrottling.Tests.Utils
{
    using System.IO;
    using System.Reflection;
    using System.Web;
    using Xunit.Sdk;

    public class CurrentHttpContextAttribute : BeforeAfterTestAttribute
    {
        private readonly string fileName;
        private readonly string url;
        private readonly string queryString;
        private StringWriter stringWriter;

        public CurrentHttpContextAttribute()
            :this(null, null, null)
        {
        }

        public CurrentHttpContextAttribute(string fileName, string url, string queryString)
        {
            this.fileName = fileName ?? "example";
            this.url = url ?? "http://localhost/example";
            this.queryString = queryString ?? string.Empty;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            stringWriter = new StringWriter();
            var httpRequest = new HttpRequest(fileName, url, queryString);
            var httpResponse = new HttpResponse(stringWriter);
            HttpContext.Current = new HttpContext(httpRequest, httpResponse);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            stringWriter.Dispose();
        }
    }
}
