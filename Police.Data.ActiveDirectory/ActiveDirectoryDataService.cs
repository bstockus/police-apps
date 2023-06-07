using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Police.Data.ActiveDirectory {

    public class ActiveDirectoryDataService {

        private readonly string _ldapConnectionString;

        public ActiveDirectoryDataService(string ldapConnectionString) {
            _ldapConnectionString = ldapConnectionString;
        }

        public async Task<IEnumerable<ActiveDirectoryUserInfo>> FetchUserInfo(CancellationToken cancellationToken) =>
            await Task.Run(() => {
                var directoryEntry = new DirectoryEntry(_ldapConnectionString);
                var directorySearcher = new DirectorySearcher(directoryEntry) {
                    SearchScope = SearchScope.Subtree,
                    Asynchronous = true,
                    CacheResults = true,
                    Sort = {
                        Direction = SortDirection.Ascending,
                        PropertyName = "name"
                    },
                    Filter = "(&" +
                             "(objectClass=user)" +
                             "(objectClass=person)" +
                             "(employeeID=*))",
                    PropertiesToLoad = {
                        "objectSid",
                        "mail",
                        "department",
                        "telephoneNumber",
                        "employeeID",
                        "title"
                    }

                };

                var results = (directorySearcher.FindAll()).Cast<SearchResult>().ToList();

                return results.Select(_ => new ActiveDirectoryUserInfo {
                    Sid = _.Properties["objectSid"]?.GetFirstSidStringValue() ?? "",
                    Department = _.Properties["department"]?.GetFirstStringValue() ?? "",
                    EmployeeId = _.Properties["employeeID"]?.GetFirstStringValue() ?? "",
                    Mail = _.Properties["mail"]?.GetFirstStringValue() ?? "",
                    TelephoneNumber = _.Properties["telephoneNumber"]?.GetFirstStringValue() ?? "",
                    Title = _.Properties["title"]?.GetFirstStringValue() ?? ""
                });
            }, cancellationToken);

    }

}