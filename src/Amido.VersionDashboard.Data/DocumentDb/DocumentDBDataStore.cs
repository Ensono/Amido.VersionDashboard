// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Linq;
using Amido.VersionDashboard.Data.Model;
using Amido.VersionDashboard.Web.Domain;
using Microsoft.Azure.Documents.Client;

namespace Amido.VersionDashboard.Data.DocumentDb {
    public class DocumentDbDataStore : IDataStore {
        private readonly string _connectionString;
        private IEnumerable<Dashboard> _dashboards;
        private DateTime _dashboardLoaded = DateTime.MinValue;
        private readonly TimeSpan _cacheLife = TimeSpan.FromHours(1);

        public DocumentDbDataStore(IConfigProvider configProvider) {
            _connectionString = configProvider.GetSetting("ConnectionString");
            TimeSpan.TryParse(configProvider.GetSetting("CacheDuration"), out _cacheLife);
        }

        public IEnumerable<Dashboard> Dashboards() {
            if (_dashboards != null && _dashboardLoaded.Add(_cacheLife) < DateTime.UtcNow) {
                return _dashboards;
            }

            var connection = new DocumentDbConnectionString(_connectionString);

            using (var client = new DocumentClient(connection.AccountEndpoint, connection.AccountKey)) {
                var database = client.ReadDatabaseFeedAsync().Result;
                var dashboardCollection =
                    client.ReadDocumentCollectionFeedAsync(
                        database.Single(x => x.Id == connection.DatabaseName).CollectionsLink).Result;
                var dashboards =
                    client.ReadDocumentFeedAsync(dashboardCollection.Single(x => x.Id == "dashboards").DocumentsLink)
                        .Result;
                _dashboards = dashboards.Select(x => new Dashboard {
                    Title = x.GetPropertyValue<string>("Title"),
                    Slug = x.GetPropertyValue<string>("Slug"),
                    Sections = x.GetPropertyValue<IEnumerable<DashboardSection>>("Sections")
                }).OrderBy(x => x.Title);
                _dashboardLoaded = DateTime.UtcNow;
                return _dashboards;
            }
        }

        public IEnumerable<Uri> AllowedUris() {
            var dashboards = Dashboards();
            return dashboards
                .SelectMany(x => x.Sections)
                .SelectMany(x => x.Panels)
                .Select(x => x.DataUri)
                .Select(x => new Uri(x));
        }

        public IDictionary<Uri, string> PinnedCertificates() {
            var dashboards = Dashboards();
            return dashboards
              .SelectMany(x => x.Sections)
              .SelectMany(x => x.Panels)
              .Where(x => x.Protocol == "HTTPS")
              .ToDictionary(x => new Uri(x.DataUri), x => x.ServerCertificateSubject);
        }
    }
}