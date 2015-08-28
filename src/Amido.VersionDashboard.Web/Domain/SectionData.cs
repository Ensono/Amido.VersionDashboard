// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Collections.Generic;

namespace Amido.VersionDashboard.Web.Domain {
    public class SectionData {
        public string Title { get; set; }
        public string Slug { get; set; }
        public IEnumerable<PanelData> Panels { get; set; }
    }
}