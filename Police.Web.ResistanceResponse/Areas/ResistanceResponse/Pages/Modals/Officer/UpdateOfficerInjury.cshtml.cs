using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Officer {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class UpdateOfficerInjuryModel : ModalPageModel {

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

        public IncidentOfficerInfo IncidentOfficerInfo { get; set; }

        public UpdateOfficerInjuryModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGet(Guid incidentId, Guid officerId) {

            IncidentId = incidentId;
            OfficerId = officerId;

            var officer = await Mediator.Send(new FetchIncidentOfficerQuery(incidentId, officerId));
            IncidentOfficerInfo = officer;

            WasOfficerInjured = officer.WasOfficerInjured;
            DidOfficerRequireMedicalAttention = officer.DidOfficerRequireMedicalAttention;
            DidOfficerRequireMedicalAttentionDescription = officer.DidOfficerRequireMedicalAttentionDescription;

        }

        public async Task<IActionResult> OnPostAsync() {

            var userId = await FetchCurrentUserId();

            await Mediator.Send(new UpdateIncidentOfficerInjuryCommand {
                IncidentId = IncidentId,
                OfficerId = OfficerId,
                WasOfficerInjured = WasOfficerInjured,
                DidOfficerRequireMedicalAttention =
                    WasOfficerInjured.Equals(YesNo.Yes) ? DidOfficerRequireMedicalAttention : YesNo.No,
                DidOfficerRequireMedicalAttentionDescription =
                    WasOfficerInjured.Equals(YesNo.Yes)
                        ? DidOfficerRequireMedicalAttentionDescription ?? ""
                        : "",
                SubmitterId = userId
            });

            return RedirectToReferrer();

        }

    }

}
