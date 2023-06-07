using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Incident {
    
    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class UpdateIncidentNumberModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public string IncidentNumber { get; set; }

        public UpdateIncidentNumberModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<IActionResult> OnGet(Guid incidentId) {

            var incidentInfo = await Mediator.Send(new FetchIncidentQuery(incidentId));

            IncidentId = incidentInfo.Id;
            IncidentNumber = incidentInfo.IncidentCaseNumber;

            return Page();

        }

        public async Task<IActionResult> OnPostAsync() {
            await Mediator.Send(new UpdateIncidentCaseNumberCommand {
                IncidentId = IncidentId,
                NewIncidentCaseNumber = IncidentNumber,
                SubmitterId = await FetchCurrentUserId()
            });

            return RedirectToReferrer();
        }

    }
}
