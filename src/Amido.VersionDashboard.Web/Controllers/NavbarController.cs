// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Linq;
using System.Web.Mvc;
using Amido.VersionDashboard.Web.Domain;

namespace Amido.VersionDashboard.Web.Controllers {
    public class NavbarController : Controller {
        // GET: Navbar
        public ActionResult Index() {
            var data = new DocumentDBDataStore();
            return PartialView("_Navbar", data.NavigationItems().ToList());
        }
    }
}