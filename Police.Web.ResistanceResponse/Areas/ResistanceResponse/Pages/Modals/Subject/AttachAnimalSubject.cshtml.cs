using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents.Subjects.Animals;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Subject {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class AttachAnimalSubjectModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Species Species { get; set; }

        public AttachAnimalSubjectModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public void OnGet(Guid incidentId) {
            IncidentId = incidentId;
        }

        public async Task<IActionResult> OnPostAsync() {

            var userId = await FetchCurrentUserId();

            await Mediator.Send(new AttachAnimalSubjectCommand {
                IncidentId = IncidentId,
                Species = Species,
                SubmitterId = userId
            });

            return RedirectToReferrer();

        }

    }

}
