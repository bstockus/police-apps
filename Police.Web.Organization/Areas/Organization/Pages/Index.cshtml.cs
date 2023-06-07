using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Organization;
using Police.Business.Organization.Officers;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.Organization.Areas.Organization.Pages {

    [Authorize(OrganizationAuthorizationPolicies.IsAllowedToViewUsers)]
    public class IndexModel : BasePageModel {

        [TempData]
        public string Message { get; set; }

        public IEnumerable<OfficerInfo> OfficerInfos { get; set; }
        public FetchAllOfficerCountsByFiltersQuery.Results OfficerCounts { get; set; }
        public UserInformation UserInformation { get; set; }

        public bool ShowingOnlyCurrentlyEmployedOfficers { get; set; }
        public bool ShowingOnlySwornOfficers { get; set; }

        public IndexModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGet(bool onlyCurrentlyEmployed = false, bool onlySworn = false, CancellationToken cancellationToken = default) {

            ShowingOnlyCurrentlyEmployedOfficers = onlyCurrentlyEmployed;
            ShowingOnlySwornOfficers = onlySworn;

            OfficerCounts = await Mediator.Send(
                new FetchAllOfficerCountsByFiltersQuery(), cancellationToken);

            OfficerInfos =
                (await Mediator.Send(
                    new FetchAllOfficersWithActiveAndSwornFiltersQuery(ShowingOnlyCurrentlyEmployedOfficers,
                        ShowingOnlySwornOfficers), cancellationToken)).ToList();
            UserInformation = await FetchCurrentUser();

        }

    }

}
