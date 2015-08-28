// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System.Configuration;
using Amido.VersionDashboard.Web.Domain;

namespace Amido.VersionDashboard.Data {
    public class AppSettingsConfigProvider : IConfigProvider {
        public string GetSetting(string appSetting) {
            return ConfigurationManager.AppSettings[appSetting];
        }
    }
}