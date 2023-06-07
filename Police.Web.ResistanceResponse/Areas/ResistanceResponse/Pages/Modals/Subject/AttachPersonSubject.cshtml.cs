using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.Subjects.People;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Subject {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class AttachPersonSubjectModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public string FullName { get; set; }
        
        [BindProperty]
        public Gender Gender { get; set; }

        [BindProperty]
        public Race Race { get; set; }

        [BindProperty]
        public SuspectedUse SuspectedUse { get; set; }

        [BindProperty]
        public YesNo WasSubjectInjured { get; set; }

        [BindProperty]
        public YesNo DidSubjectRequireMedicalAttention { get; set; }

        [BindProperty]
        public string DidSubjectRequireMedicalAttentionDescription { get; set; }

        [BindProperty]
        public DateTime? DateOfBirth { get; set; }

        public AttachPersonSubjectModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public void OnGet(Guid incidentId) {
            IncidentId = incidentId;
        }

        public async Task<IActionResult> OnPostAsync() {

            var userId = await FetchCurrentUserId();

            var incident = await Mediator.Send(new FetchIncidentQuery(IncidentId));

            var age = (int?) null;

            if (DateOfBirth.HasValue) {
                var dateOfBirth = LocalDate.FromDateTime(DateOfBirth.Value);

                age = Math.Abs(Period.Between(incident.IncidentDateAndTime.Date, dateOfBirth).Years);
            }

            

            await Mediator.Send(new AttachPersonSubjectCommand {
                IncidentId = IncidentId,
                FullName = FullName,
                Age = age,
                Gender = Gender,
                Race = Race,
                SuspectedUse = SuspectedUse,
                WasSubjectInjured = WasSubjectInjured,
                DidSubjectRequireMedicalAttention = DidSubjectRequireMedicalAttention,
                DidSubjectRequireMedicalAttentionDescription = DidSubjectRequireMedicalAttentionDescription ?? "",
                SubmitterId = userId,
                DateOfBirth = DateOfBirth.HasValue ? LocalDate.FromDateTime(DateOfBirth.Value) : (LocalDate?)null
            });

            return RedirectToReferrer();

        }

    }

}
