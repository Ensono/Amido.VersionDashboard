// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Web.Mvc;
using System.Web.Routing;

namespace Amido.VersionDashboard.Web {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Dashboard", "dashboard/{slug}", new {controller = "Dashboard", action = "Dashboard"}
                );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}