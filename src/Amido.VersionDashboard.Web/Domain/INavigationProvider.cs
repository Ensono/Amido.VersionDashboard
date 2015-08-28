// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Collections.Generic;
using Amido.VersionDashboard.Web.Models;

namespace Amido.VersionDashboard.Web.Domain {
    public interface INavigationProvider {
        IEnumerable<Navbar> NavigationItems();
    }
}