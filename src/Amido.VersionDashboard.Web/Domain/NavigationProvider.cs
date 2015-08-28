// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Collections.Generic;
using System.Linq;
using Amido.VersionDashboard.Data;
using Amido.VersionDashboard.Web.Models;

namespace Amido.VersionDashboard.Web.Domain {
    public class NavigationProvider : INavigationProvider {
        private readonly IDataStore _dataStore;

        public NavigationProvider(IDataStore dataStore) {
            _dataStore = dataStore;
        }

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
            var dashboards = _dataStore.Dashboards();

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
    }
}