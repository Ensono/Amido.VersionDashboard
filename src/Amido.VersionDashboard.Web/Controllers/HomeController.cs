// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Web.Mvc;

namespace Amido.VersionDashboard.Web.Controllers {
    public class HomeController : AsyncController {
        public ActionResult Index() {
            return View("Index");
        }

        public ActionResult Credits() {
            return View("Credits");
        }
    }
}