
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Business.ResistanceResponse.Incidents.Subjects.Animals;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Subject {
    
    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class UpdateAnimalSubjectModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Guid SubjectId { get; set; }
        
        [BindProperty]
        public Species Species { get; set; }
        
        public UpdateAnimalSubjectModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<IActionResult> OnGet(Guid incidentId, Guid subjectId) {

            var subjectInfo = await Mediator.Send(new FetchSubjectQuery(incidentId, subjectId));

            if (subjectInfo is SubjectAnimalInfo subjectAnimalInfo) {
                
                IncidentId = incidentId;
                SubjectId = subjectId;
                Species = subjectAnimalInfo.Species;

                return Page();
            }
                
            return BadRequest();

        }

        public async Task<IActionResult> OnPostAsync() {

            await Mediator.Send(new UpdateAnimalSubjectCommand {
                IncidentId = IncidentId,
                SubjectId = SubjectId,
                Species = Species,
                SubmitterId = await FetchCurrentUserId()
            });

            return RedirectToReferrer();

        }

    }

}