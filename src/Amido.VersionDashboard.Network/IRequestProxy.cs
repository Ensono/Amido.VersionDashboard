// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Threading.Tasks;

namespace Amido.VersionDashboard.Network {
    public interface IRequestProxy {
        Task<string> GetData(Uri uri, string responsePath, string trustInvalidCertificatesWithSubject = null);
    }
}