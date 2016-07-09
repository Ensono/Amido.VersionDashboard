// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Amido.VersionDashboard.Data;
using Amido.VersionDashboard.Network;
using Amido.VersionDashboard.Web.Models;

namespace Amido.VersionDashboard.Web.Controllers {
    public class ProxyController : ApiController {
        private readonly IDataStore _dataStore;
        private readonly IRequestProxy _requestProxy;

        public ProxyController(IDataStore dataStore, IRequestProxy requestProxy) {
            _dataStore = dataStore;
            _requestProxy = requestProxy;
        }

        [HttpGet]
        public async Task<ResponseModel> Get(Uri uri, string responsePath) {
            if (!_dataStore.AllowedUris().Contains(uri)) {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden) {
                    Content = new StringContent("The requested URI was not allowed."),
                    ReasonPhrase = "URI Forbidden"
                });
            }

            var stopwatch = Stopwatch.StartNew();
            var pinnedCertificates = _dataStore.PinnedCertificates();
            var pinnedCertificate = pinnedCertificates.SingleOrDefault(x => x.Key == uri).Value;
            var version = await _requestProxy.GetData(uri, responsePath, pinnedCertificate);
            stopwatch.Stop();
            return new ResponseModel {
                Version = version,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
            };
        }
    }
}