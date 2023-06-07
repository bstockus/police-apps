using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Business.ResistanceResponse.Incidents.Subjects.People;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.Common.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages.Modals.Subject {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class UpdatePersonSubjectModel : ModalPageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }
        
        [BindProperty]
        public Guid SubjectId { get; set; }

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
        
        [BindProperty]
        public int Age { get; set; }
        
        public UpdatePersonSubjectModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

        public async Task<IActionResult> OnGet(Guid incidentId, Guid subjectId) {

            var subjectInfo = await Mediator.Send(new FetchSubjectQuery(incidentId, subjectId));

            if (subjectInfo is SubjectPersonInfo subjectPersonInfo) {

                IncidentId = incidentId;
                SubjectId = subjectId;
                FullName = subjectPersonInfo.FullName;
                Gender = subjectPersonInfo.Gender;
                Race = subjectPersonInfo.Race;
                SuspectedUse = subjectPersonInfo.SuspectedUse;
                WasSubjectInjured = subjectPersonInfo.WasSubjectInjured;
                DidSubjectRequireMedicalAttention = subjectPersonInfo.DidSubjectRequireMedicalAttention;
                DidSubjectRequireMedicalAttentionDescription =
                    subjectPersonInfo.DidSubjectRequireMedicalAttentionDescription;
                DateOfBirth = subjectPersonInfo.DateOfBirth?.ToDateTimeUnspecified();
                Age = subjectPersonInfo.Age ?? 0;

                return Page();

            }

            return BadRequest();

        }

        public async Task<IActionResult> OnPostAsync() {

            int age = 0;
            
            if (DateOfBirth.HasValue) {
                var incident = await Mediator.Send(new FetchIncidentQuery(IncidentId));

                var dateOfBirth = LocalDate.FromDateTime(DateOfBirth.Value);

                age = Math.Abs(Period.Between(incident.IncidentDateAndTime.Date, dateOfBirth).Years);
            } else {
                age = Age;
            }

            await Mediator.Send(new UpdatePersonSubjectCommand {
                IncidentId = IncidentId,
                SubjectId = SubjectId,
                SubmitterId = await FetchCurrentUserId(),
                FullName = FullName,
                Gender = Gender,
                Race = Race,
                SuspectedUse = SuspectedUse,
                WasSubjectInjured = WasSubjectInjured,
                DidSubjectRequireMedicalAttention = DidSubjectRequireMedicalAttention,
                DidSubjectRequireMedicalAttentionDescription = DidSubjectRequireMedicalAttentionDescription ?? "",
                DateOfBirth = DateOfBirth.HasValue ? LocalDate.FromDateTime(DateOfBirth.Value) : (LocalDate?)null,
                Age = age
            });

            return RedirectToReferrer();

        }

    }

}