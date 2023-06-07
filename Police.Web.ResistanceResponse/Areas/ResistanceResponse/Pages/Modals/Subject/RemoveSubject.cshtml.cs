using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Subject {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class RemoveSubjectModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Guid SubjectId { get; set; }

        public SubjectInfo SubjectInfo { get; set; }

        public RemoveSubjectModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGet(Guid incidentId, Guid subjectId) {

            IncidentId = incidentId;
            SubjectId = subjectId;

            SubjectInfo = await Mediator.Send(new FetchSubjectQuery(incidentId, subjectId));

        }

        public async Task<ActionResult> OnPostAsync() {

            await Mediator.Send(new RemoveSubjectCommand {
                IncidentId = IncidentId,
                SubjectId = SubjectId
            });

            return RedirectToReferrer();

        }

    }

}
