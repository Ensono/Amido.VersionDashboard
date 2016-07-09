// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amido.VersionDashboard.Data.Model;
using Newtonsoft.Json;

namespace Amido.VersionDashboard.Data.FileSystem {
    public class FileSystemDataStore : IDataStore {
        private readonly string _rootPath;

        public FileSystemDataStore(string rootPath) {
            _rootPath = rootPath;
        }

        public IEnumerable<Uri> AllowedUris() {
            var dashboards = Dashboards();
            return dashboards
                .SelectMany(x => x.Sections)
                .SelectMany(x => x.Panels)
                .Select(x => x.DataUri)
                .Select(x => new Uri(x));
        }

        public IEnumerable<Dashboard> Dashboards() {
            var data = Path.Combine(_rootPath, "TestData");
            return Directory.GetFiles(data)
                .Select(File.ReadAllText)
                .Select(JsonConvert.DeserializeObject<Dashboard>);
        }

        public IDictionary<Uri, string> PinnedCertificates() {
            return new Dictionary<Uri, string>();
        }
    }
}