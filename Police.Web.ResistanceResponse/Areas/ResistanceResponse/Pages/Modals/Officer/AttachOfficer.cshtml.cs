using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Police.Business.Common;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Officer {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class AttachOfficerModel : ModalPageModel {

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

        public IEnumerable<SelectListItem> OfficerSelectListItems { get; set; }

        public AttachOfficerModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task OnGet(Guid incidentId) {

            IncidentId = incidentId;

            var incidentDetailedInformation = await Mediator.Send(new FetchDetailedIncidentQuery(incidentId));
            var officerInfos = await Mediator.Send(new FetchAllActiveSwornOfficersQuery());

            var incidentOfficers = incidentDetailedInformation.IncidentOfficers.Select(_ => _.OfficerId).ToList();

            OfficerSelectListItems = officerInfos
                .Where(_ => !incidentOfficers.Any(x => x.Equals(_.Id)))
                .OrderBy(_ => _.LastName).ThenBy(_ => _.FirstName)
                .Select(_ => new SelectListItem($"{_.LastName}, {_.FirstName} ({_.BadgeNumber})", _.Id.ToString()));

        }

        public async Task<ActionResult> OnPostAsync() {

            var userId = await FetchCurrentUserId();

            await Mediator.Send(new AttachIncidentOfficerCommand {
                IncidentId = IncidentId,
                OfficerId = OfficerId,
                WasOfficerInjured = WasOfficerInjured,
                DidOfficerRequireMedicalAttention = DidOfficerRequireMedicalAttention,
                DidOfficerRequireMedicalAttentionDescription = DidOfficerRequireMedicalAttentionDescription ?? "",
                SubmitterId = userId
            });

            return RedirectToReferrer();

        }

    }

}
