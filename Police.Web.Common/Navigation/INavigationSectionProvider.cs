using System.Threading.Tasks;

namespace Police.Web.Common.Navigation {

    public interface INavigationSectionProvider {

        Task<NavigationSection> GenerateNavigationSection();

    }

}
