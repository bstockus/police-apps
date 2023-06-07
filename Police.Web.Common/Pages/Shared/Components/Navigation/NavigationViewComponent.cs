using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Police.Security.User;
using Police.Web.Common.Navigation;

namespace Police.Web.Common.Pages.Shared.Components.Navigation {

    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : ViewComponent {

        public class NavBarViewModel {

            public UserInformation User { get; set; }
            public IEnumerable<NavigationSection> Sections { get; set; }

        }

        private readonly IEnumerable<INavigationSectionProvider> _navigationSectionProviders;
        private readonly IUserService _userService;

        public NavigationViewComponent(
            IEnumerable<INavigationSectionProvider> navigationSectionProviders,
            IUserService userService) {

            _navigationSectionProviders = navigationSectionProviders;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool forHomePage = false) {

            var navigationSections = new List<NavigationSection>();

            foreach (var navigationSectionProvider in _navigationSectionProviders) {

                navigationSections.Add(await navigationSectionProvider.GenerateNavigationSection());

            }

            if (forHomePage) {
                return View("ForHomePage", navigationSections);
            }

            return View(new NavBarViewModel {
                User = await _userService.GetUserInformationForClaimsPrincipal(HttpContext.User),
                Sections = navigationSections
            });

        }

    }

}
