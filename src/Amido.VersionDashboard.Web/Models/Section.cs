// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Collections.Generic;

namespace Amido.VersionDashboard.Web.Models {
    public class Section {
        public Section() {
            Panels = new List<VersionPanel>();
        }

        public string Title { get; set; }
        public IEnumerable<VersionPanel> Panels { get; set; }
    }
}