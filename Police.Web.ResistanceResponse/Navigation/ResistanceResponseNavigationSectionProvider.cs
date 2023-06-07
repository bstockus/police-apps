using System.Threading.Tasks;
using Police.Web.Common.Navigation;
using Police.Web.ResistanceResponse.Constants;

namespace Police.Web.ResistanceResponse.Navigation {

    public class ResistanceResponseNavigationSectionProvider : INavigationSectionProvider {

        public async Task<NavigationSection> GenerateNavigationSection() =>
            await Task.FromResult(new NavigationSection(
                "Resistance Response",
                new NavigationItem("Add or Update Report", "Add/Update", AreaConstants.ResistanceReport, PageConstants.Index),
                new NavigationItem("View Reports", "View", AreaConstants.ResistanceReport, PageConstants.List)));

    }

}
