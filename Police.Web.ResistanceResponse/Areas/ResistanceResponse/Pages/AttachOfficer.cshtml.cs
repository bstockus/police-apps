using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Common;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class AttachOfficerModel : ResistanceResponsePageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Guid OfficerId { get; set; }

        [BindProperty]
        public YesNo WasOfficerInjured { get; set; }

        [BindProperty]
        public YesNo DidOfficerRequireMedicalAttention { get; set; }

        [BindProperty]
        public string DidOfficerRequireMedicalAttentionDescription { get; set; }

        public IncidentInfo Incident { get; set; }

        public OfficerInfo Officer { get; set; } 

        public IEnumerable<OfficerInfo> OfficerInfos { get; set; }

        public AttachOfficerModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGetAsync(Guid incidentId) {

            IncidentId = incidentId;

            Incident = await Mediator.Send(new FetchIncidentQuery(incidentId));
            Officer = await Mediator.Send(new FetchOfficerByUserIdQuery(await FetchCurrentUserId()));

            OfficerId = Officer.Id;

            if ((await UserService.GetUserInformationForClaimsPrincipal(User))
                .CanUserSubmitForOfficersOtherThanThemselves()) {

                OfficerInfos = await Mediator.Send(new FetchAllActiveSwornOfficersQuery());

            }

        }

        public async Task<IActionResult> OnPostAsync() {

            var userId = await FetchCurrentUserId();

            await Mediator.Send(new AttachIncidentOfficerCommand {
                IncidentId = IncidentId,
                OfficerId = OfficerId,
                WasOfficerInjured = WasOfficerInjured,
                DidOfficerRequireMedicalAttention = DidOfficerRequireMedicalAttention,
                DidOfficerRequireMedicalAttentionDescription = DidOfficerRequireMedicalAttentionDescription ?? "",
                SubmitterId = userId
            });

            return RedirectToDetailsPage(IncidentId);

        }

    }
}
