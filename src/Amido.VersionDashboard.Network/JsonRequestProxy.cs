// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Amido.VersionDashboard.Network {
    public class JsonRequestProxy : IRequestProxy {
        public async Task<string> GetData(Uri uri, string responsePath) {
            using (var client = new HttpClient()) {
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
    }
}