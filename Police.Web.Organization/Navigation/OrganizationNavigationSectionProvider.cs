using System.Threading.Tasks;
using Police.Web.Common.Navigation;
using Police.Web.Organization.Constants;

namespace Police.Web.Organization.Navigation {

    public class OrganizationNavigationSectionProvider : INavigationSectionProvider {

        public async Task<NavigationSection> GenerateNavigationSection() =>
            await Task.FromResult(
                new NavigationSection(
                    "Organization",
                    new NavigationItem(
                        "View Officers",
                        "Officers",
                        AreaConstants.Organization,
                        PageConstants.Index)));

    }

}
