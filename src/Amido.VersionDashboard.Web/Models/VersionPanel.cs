// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;

namespace Amido.VersionDashboard.Web.Models {
    public class VersionPanel {
        public string IconClass { get; set; }
        public string FlagClass { get; set; }
        public Uri DataUri { get; set; }
        public string JsonQuery { get; set; }
        public string Description { get; set; }
        public string Protocol { get; set; }
    }
}