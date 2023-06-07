using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Report {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class RemoveReportModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Guid SubjectId { get; set; }

        [BindProperty]
        public Guid OfficerId { get; set; }

        public ReportInfo ReportInfo { get; set; }

        public RemoveReportModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGet(Guid incidentId, Guid subjectId, Guid officerId) {

            IncidentId = incidentId;
            SubjectId = subjectId;
            OfficerId = officerId;

            ReportInfo = await Mediator.Send(new FetchReportQuery(incidentId, subjectId, officerId));

        }

        public async Task<ActionResult> OnPostAsync() {

            await Mediator.Send(new RemoveReportCommand {
                IncidentId = IncidentId,
                OfficerId = OfficerId,
                SubjectId = SubjectId
            });

            return RedirectToReferrer();

        }


    }

}
