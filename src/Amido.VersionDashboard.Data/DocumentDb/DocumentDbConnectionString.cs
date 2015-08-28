// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Linq;

namespace Amido.VersionDashboard.Data.DocumentDb {
    public class DocumentDbConnectionString {
        public DocumentDbConnectionString(string connectionString) {
            var parts = connectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = parts.ToDictionary(key => key.Substring(0, key.IndexOf('=')),
                value => value.Substring(value.IndexOf('=') + 1));

            if (dictionary.ContainsKey("AccountEndpoint")) {
                AccountEndpoint = new Uri(dictionary["AccountEndpoint"], UriKind.Absolute);
            }

            if (dictionary.ContainsKey("AccountKey")) {
                AccountKey = dictionary["AccountKey"];
            }

            if (dictionary.ContainsKey("DatabaseName")) {
                DatabaseName = dictionary["DatabaseName"];
            }
        }

        public Uri AccountEndpoint { get; set; }

        public string AccountKey { get; set; }

        public string DatabaseName { get; set; }

        public override string ToString() {
            if (string.IsNullOrWhiteSpace(DatabaseName)) {
                return $"AccountEndpoint={AccountEndpoint};AccountKey={AccountKey};";
            }

            return $"AccountEndpoint={AccountEndpoint};AccountKey={AccountKey};DatabaseName={DatabaseName}";
        }
    }
}