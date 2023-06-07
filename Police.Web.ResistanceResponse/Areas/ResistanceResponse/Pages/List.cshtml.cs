using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class ListModel : ResistanceResponsePageModel {

        [TempData]
        public string Message { get; set; }

        public ListModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public UserInformation UserInformation { get; set; }

        public IEnumerable<IncidentListInfo> IncidentListInfos { get; set; }
        public IEnumerable<FetchAllIncidentYearsAndCountsQuery.IncidentYearsAndCounts> IncidentYearsAndCounts { get; set; }
        public int YearToDisplay { get; set; }

        public async Task OnGet(int? year) {

            UserInformation = await FetchCurrentUser();

            IncidentYearsAndCounts =
                (await Mediator.Send(new FetchAllIncidentYearsAndCountsQuery()))
                .ToList();

            YearToDisplay = year ?? (IncidentYearsAndCounts.OrderByDescending(_ => _.Year).FirstOrDefault()?.Year ?? 0);

            IncidentListInfos =
                (await Mediator.Send(new FetchAllListIncidentsForYearQuery(YearToDisplay)))
                .Where(_ => _.CanUserViewIncidentOrAnyOfItsChilderen(UserInformation))
                .ToList();

        }

    }

}
