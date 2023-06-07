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
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Incident {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class UpdateIncidentDateAndTimeModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public DateTime IncidentDate { get; set; }

        [BindProperty]
        public string IncidentTime { get; set; }


        public UpdateIncidentDateAndTimeModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<IActionResult> OnGet(Guid incidentId) {

            var incidentInfo = await Mediator.Send(new FetchIncidentQuery(incidentId));

            IncidentId = incidentInfo.Id;
            IncidentDate = incidentInfo.IncidentDateAndTime.ToDateTimeUnspecified();
            IncidentTime = incidentInfo.IncidentDateAndTime.ToDateTimeUnspecified().ToShortTimeString();

            return Page();

        }

        public async Task<IActionResult> OnPostAsync() {

            var localTime = (LocalTimePattern.Create("h:mm tt", CultureInfo.InvariantCulture)).Parse(IncidentTime);
            var localDateTime = localTime.Value.On(LocalDate.FromDateTime(IncidentDate));

            await Mediator.Send(new UpdateIncidentDateAndTimeCommand() {
                IncidentId = IncidentId,
                NewIncidentDateAndTime = localDateTime,
                SubmitterId = await FetchCurrentUserId()
            });

            return RedirectToReferrer();
        }

    }
}
