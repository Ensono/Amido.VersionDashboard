// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Amido.VersionDashboard.Web.Domain;
using Newtonsoft.Json.Linq;

namespace Amido.VersionDashboard.Network.Json {
    public class JsonRequestProxy : IRequestProxy {
        private readonly IConfigProvider _configProvider;
        public JsonRequestProxy(IConfigProvider configProvider) {
            _configProvider = configProvider;
        }

        public async Task<string> GetData(Uri uri, string responsePath, string trustInvalidCertificatesWithSubject = null) {
            var handler = new WebRequestHandler();
            
            handler.ServerCertificateValidationCallback += OnValidation(trustInvalidCertificatesWithSubject);
            using (var client = trustInvalidCertificatesWithSubject != null ? new HttpClient(handler) : new HttpClient()) {
                var response = await client.GetAsync(uri);
                if (response.StatusCode == HttpStatusCode.OK) {
                    var content = await response.Content.ReadAsStringAsync();
                    var document = JObject.Parse(content);
                    return document.SelectToken(responsePath).Value<string>();
                }

                var errorResponse = new HttpResponseMessage(response.StatusCode) {
                    Content = response.Content,
                    ReasonPhrase = response.StatusCode.ToString()
                };

                throw new HttpResponseException(errorResponse);
            }
        }

        private static RemoteCertificateValidationCallback OnValidation(string certificateSubject)
        {
            return
                (sender, certificate, chain, errors) =>
                    certificate.Subject.Equals(certificateSubject, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}