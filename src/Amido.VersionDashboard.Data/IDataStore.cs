// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using Amido.VersionDashboard.Data.Model;

namespace Amido.VersionDashboard.Data {
    public interface IDataStore {
        IEnumerable<Uri> AllowedUris();
        IEnumerable<Dashboard> Dashboards();
    }
}