// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Amido.VersionDashboard.Network.FileSystem {
    public class FileSystemRequestProxy : IRequestProxy {
        private readonly string _rootPath;

        public FileSystemRequestProxy(string rootPath) {
            _rootPath = rootPath;
        }

        public Task<string> GetData(Uri uri, string responsePath, string trustInvalidCertificatesWithSubject = null) {
            var endpointsDirectory = Path.Combine(_rootPath, "Endpoints");
            var supportedHosts = Directory.GetDirectories(endpointsDirectory)
                .Select(x => new DirectoryInfo(x))
                .ToList();

            if (supportedHosts.All(x => x.Name != uri.Host)) {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.NotFound) {
                    Content = new StringContent("The requested URL could not be reached."),
                    ReasonPhrase = "NotFound"
                };

                throw new HttpResponseException(errorResponse);
            }

            var targetPattern = string.Format("{0}.json", uri.LocalPath.Remove(0, 1));
            var hostDirectory = supportedHosts.Single(x => x.Name == uri.Host).FullName;
            var responseData = Directory.GetFiles(hostDirectory, targetPattern)
                .Select(x => new FileInfo(x))
                .Single();

            var content = File.ReadAllText(responseData.FullName);
            var data = JObject.Parse(content);

            return Task.FromResult(data.SelectToken(responsePath).Value<string>());
        }
    }
}