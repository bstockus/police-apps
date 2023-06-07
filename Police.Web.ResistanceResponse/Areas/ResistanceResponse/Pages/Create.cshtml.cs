using System;
using System.Globalization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using NodaTime.Text;
using Police.Business.ResistanceResponse.Incidents;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class CreateModel : ResistanceResponsePageModel {

        [BindProperty]
        public string CaseNumber { get; set; }

        [BindProperty]
        public DateTime IncidentDate { get; set; }

        [BindProperty]
        public string IncidentTime { get; set; }

        public CreateModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public void OnGet(string caseNumber) {
            CaseNumber = caseNumber ?? "";
            IncidentDate = DateTime.Now;
            IncidentTime = DateTime.Now.ToShortTimeString();
        }

        public async Task<ActionResult> OnPostAsync() {
            var localTime = (LocalTimePattern.Create("h:mm tt", CultureInfo.InvariantCulture)).Parse(IncidentTime);
            var localDateTime = localTime.Value.On(LocalDate.FromDateTime(IncidentDate));

            var incidentId = await Mediator.Send(new CreateIncidentCommand {
                IncidentCaseNumber = CaseNumber,
                IncidentDateAndTime = localDateTime,
                SubmitterId = await FetchCurrentUserId()
            });

            return RedirectToAttachOfficerPage(incidentId);
        }

    }

}
