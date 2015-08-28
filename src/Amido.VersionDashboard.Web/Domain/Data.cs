// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Amido.VersionDashboard.Web.Models;
using Microsoft.Azure.Documents.Client;

namespace Amido.VersionDashboard.Web.Domain {
    public class Data {
        private string DocumentDatabaseUri = "TODO";
        private string DocumentDatabaseAuthKey = "TODO";
        private string DocumentDatabaseName = "TODO";
        private IEnumerable<DashboardData> _dashboardCache;

        public IEnumerable<Navbar> NavigationItems() {
            var menu = new List<Navbar> {
                new Navbar {
                    Id = 1,
                    nameOption = "Home",
                    controller = "Home",
                    action = "Index",
                    imageClass = "fa fa-home fa-fw",
                    status = true,
                    isParent = false,
                    parentId = 0
                },
                new Navbar {
                    Id = 99,
                    nameOption = "Credits",
                    controller = "Home",
                    action = "Credits",
                    imageClass = "fa fa-creative-commons fa-fw",
                    status = true,
                    isParent = false,
                    parentId = 0
                }
            };

            var index = 2;
            var dashboards = Dashboards();

            menu.AddRange(dashboards.Select(dashboard => new Navbar {
                Id = index++,
                nameOption = dashboard.Title,
                controller = "Dashboard",
                action = dashboard.Slug,
                imageClass = "fa fa-dashboard fa-fw",
                status = true,
                isParent = false,
                parentId = 0
            }));

            return menu.OrderBy(x => x.Id).ToList();
        }

        public IEnumerable<DashboardData> Dashboards() {
            if (_dashboardCache != null) {
                return _dashboardCache;
            }

            using (var client = new DocumentClient(new Uri(DocumentDatabaseUri), DocumentDatabaseAuthKey)) {
                var database = client.ReadDatabaseFeedAsync().Result;
                var dashboardCollection =
                    client.ReadDocumentCollectionFeedAsync(
                        database.Single(x => x.Id == DocumentDatabaseName).CollectionsLink).Result;
                var dashboards =
                    client.ReadDocumentFeedAsync(dashboardCollection.Single(x => x.Id == "dashboards").DocumentsLink)
                        .Result;
                _dashboardCache = dashboards.Select(x => new DashboardData {
                    Title = x.GetPropertyValue<string>("Title"),
                    Slug = x.GetPropertyValue<string>("Slug"),
                    Sections = x.GetPropertyValue<IEnumerable<SectionData>>("Sections")
                });

                return _dashboardCache;
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