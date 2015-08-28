// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Amido.VersionDashboard.Web.Domain;
using Amido.VersionDashboard.Web.Models;
using Newtonsoft.Json.Linq;

namespace Amido.VersionDashboard.Web.Controllers {
    public class ProxyController : ApiController {
        [HttpGet]
        public async Task<ResponseModel> Get(Uri uri, string responsePath) {
            var data = new Data();
            if (!data.AllowedUris().Contains(uri)) {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden) {
                    Content = new StringContent("The requested URI was not allowed."),
                    ReasonPhrase = "URI Forbidden"
                });
            }

            var stopwatch = Stopwatch.StartNew();

            using (var client = new HttpClient()) {
                var response = await client.GetAsync(uri);
                stopwatch.Stop();

                if (response.StatusCode == HttpStatusCode.OK) {
                    var content = await response.Content.ReadAsStringAsync();
                    var document = JObject.Parse(content);
                    return new ResponseModel {
                        Version = document.SelectToken(responsePath).Value<string>(),
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
                    };
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