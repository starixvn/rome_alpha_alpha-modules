namespace Alpha.Announcements.Client.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using System.Text;
    using Configurations;
    using RestSharp;
    using RestSharp.Authenticators;
    using RestSharp.Extensions.MonoHttp;

    public class HmacAuthenticator : IAuthenticator
    {
        private static string _machineIP;

        public HmacAuthenticator()
        {
            if (!string.IsNullOrEmpty(_machineIP))
            {
                _machineIP = GetLocalIPAddress();
            }
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Rest Client cannot be null");
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Rest Request cannot be null");
            }

            string requestUri = HttpUtility.UrlEncode(client.BuildUri(request).ToString().ToLower());
            string requestHttpMethod = request.Method.ToString();

            // Calculate UNIX time
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            // create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            // Creating the raw signature string
            string signatureRawData = $"{_machineIP}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}";
            var secretKeyByteArray = Convert.FromBase64String(AppSettings.SecretKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                // Setting the values in the Authorization header using custom scheme (amx)
                string authHeader = $"amx {_machineIP}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}";
                request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
            }
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString().Replace(":", "_");
                }
            }

            throw new InvalidOperationException("Local IP Address Not Found!");
        }
    }
}