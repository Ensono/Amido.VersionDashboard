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

        public DocumentDbDataStore(IConfigProvider configProvider) {
            _connectionString = configProvider.GetSetting("ConnectionString");
        }

        public IEnumerable<Dashboard> Dashboards() {
            var connection = new DocumentDbConnectionString(_connectionString);

            using (var client = new DocumentClient(connection.AccountEndpoint, connection.AccountKey)) {
                var database = client.ReadDatabaseFeedAsync().Result;
                var dashboardCollection =
                    client.ReadDocumentCollectionFeedAsync(
                        database.Single(x => x.Id == connection.DatabaseName).CollectionsLink).Result;
                var dashboards =
                    client.ReadDocumentFeedAsync(dashboardCollection.Single(x => x.Id == "dashboards").DocumentsLink)
                        .Result;
                return dashboards.Select(x => new Dashboard {
                    Title = x.GetPropertyValue<string>("Title"),
                    Slug = x.GetPropertyValue<string>("Slug"),
                    Sections = x.GetPropertyValue<IEnumerable<DashboardSection>>("Sections")
                });
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
    }
}