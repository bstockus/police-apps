using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class IndexModel : ResistanceResponsePageModel {

        [BindProperty]
        public string CaseNumber { get; set; }

        public IndexModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<ActionResult> OnPost() {

            var incidentInfo = await Mediator.Send(new FetchIncidentByCaseNumberQuery(CaseNumber));

            if (incidentInfo == null) {
                return RedirectToCreatePage(CaseNumber);
            }

            var userInfo = await FetchCurrentUser();

            var officerInfo = await Mediator.Send(new FetchOfficerByUserIdQuery(userInfo.UserId));
                
            if (!userInfo.CanUserSubmitForOfficersOtherThanThemselves() &&
                await Mediator.Send(
                    new FetchIncidentOfficerQuery(incidentInfo.Id,
                        officerInfo.Id)) == null) {

                return RedirectToAttachOfficerPage(incidentInfo.Id);
            }

            return RedirectToDetailsPage(incidentInfo.Id);

        }

    }

}
