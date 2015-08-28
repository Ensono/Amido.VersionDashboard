// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Linq;
using System.Web.Mvc;
using Amido.VersionDashboard.Web.Domain;

namespace Amido.VersionDashboard.Web.Controllers {
    public class NavbarController : Controller {
        private readonly IDataStore _dataStore;

        public NavbarController(IDataStore dataStore) {
            _dataStore = dataStore;
        }

        // GET: Navbar
        public ActionResult Index() {
            return PartialView("_Navbar", _dataStore.NavigationItems().ToList());
        }
    }
}