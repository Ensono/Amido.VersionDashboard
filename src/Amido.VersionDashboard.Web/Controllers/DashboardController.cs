// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Amido.VersionDashboard.Data;
using Amido.VersionDashboard.Web.Models;

namespace Amido.VersionDashboard.Web.Controllers {
    public class DashboardController : Controller {
        private readonly IDataStore _dataStore;

        private readonly Dictionary<string, string> flags = new Dictionary<string, string> {
            {"Ireland", "flag flag-ireland"},
            {"Netherlands", "flag flag-netherlands"}
        };

        private readonly Dictionary<string, string> icons = new Dictionary<string, string> {
            {"Shopping Cart", "fa fa-shopping-cart fa-5x"},
            {"Delivery Options", "noun noun-delivery"},
            {"Discounts Options", "noun noun-discounts"},
            {"Vouchers Options", "noun noun-vouchers"},
            {"Subscriptions Options", "noun noun-subscriptions"}
        };

        public DashboardController(IDataStore dataStore) {
            _dataStore = dataStore;
        }

        // GET: Dashboard
        public ActionResult Dashboard(string slug) {
            var dashboardDocuments = _dataStore.Dashboards();
            var thisDashboard = dashboardDocuments.First(x => x.Slug == slug);

            var model = new Dashboard {
                Title = thisDashboard.Title,
                Slug = thisDashboard.Slug,
                Sections = thisDashboard.Sections.Select(h =>
                    new Section {
                        Title = h.Title,
                        Panels = h.Panels.Select(p =>
                            new VersionPanel {
                                FlagClass = flags[p.Flag],
                                IconClass = icons[p.Icon],
                                JsonQuery = p.JsonQuery,
                                Protocol = p.Protocol,
                                Description = p.Description,
                                DataUri = new Uri(p.DataUri)
                            }
                            )
                    }
                    )
            };

            return View("Dashboard", model);
        }
    }
}