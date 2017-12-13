namespace Alpha.Web
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public interface IRequestAnalyst
    {
        int GetCustomerId(string parameterName);
    }

    public class RequestAnalyst : IRequestAnalyst
    {
        public RequestAnalyst()
        {            
        }

        public int GetCustomerId(string parameterName)
        {
            var customerId = 0;            
            if (!string.IsNullOrEmpty(parameterName))
            {
                var jsonString = GetJsonDataInRequestBody(HttpContext.Current.Request.InputStream);
                var pattern = string.Format("(\"{0}\":)(?<paramValue>[^,]+)", parameterName);
                Match regexMatch = Regex.Match(jsonString, pattern, RegexOptions.IgnoreCase);
                if (regexMatch.Success)
                {
                    customerId = Convert.ToInt32(regexMatch.Groups["paramValue"].ToString().Replace("\"", string.Empty));
                }
            }

            return customerId;
        }

        private string GetJsonDataInRequestBody(Stream requestStream)
        {
            if (requestStream != null)
            {
                requestStream.Seek(0, SeekOrigin.Begin);
                var bodyData = new StreamReader(requestStream).ReadToEnd();
                requestStream.Seek(0, SeekOrigin.Begin);
                if (!string.IsNullOrWhiteSpace(bodyData)
                    && bodyData.Contains("{"))
                {
                    return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
                }
            }

            return string.Empty;
        }
    }
}