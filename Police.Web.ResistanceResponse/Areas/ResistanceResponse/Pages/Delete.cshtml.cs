using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Incidents;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Constants;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class DeleteModel : ResistanceResponsePageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        public IncidentDetailedInfo Incident { get; set; }
        public UserInformation UserInformation { get; set; }
        public OfficerInfo Officer { get; set; }

        public DeleteModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<ActionResult> OnGet(Guid incidentId) {

            IncidentId = incidentId;

            Incident = await Mediator.Send(new FetchDetailedIncidentQuery(incidentId));

            if (Incident == null) {
                return NotFound();
            }


            return Page();

        }

        public async Task<ActionResult> OnPost() {

            await Mediator.Send(new DeleteIncidentCommand {
                IncidentId = IncidentId,
                ApproverId = await FetchCurrentUserId()
            });

            return RedirectToPage(PageConstants.List);

        }

    }
}
