using System;
using System.Collections.Generic;
using Amido.VersionDashboard.Web.Models;

namespace Amido.VersionDashboard.Web.Domain
{
    public interface IDataStore
    {
        IEnumerable<Uri> AllowedUris();
        IEnumerable<DashboardData> Dashboards();
        IEnumerable<Navbar> NavigationItems();
    }
}