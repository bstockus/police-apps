using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.ResistanceResponse.Approvals;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class UpdatePersonSubjectCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid SubmitterId { get; set; }
        public string FullName { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        public SuspectedUse SuspectedUse { get; set; }
        public YesNo WasSubjectInjured { get; set; }
        public YesNo DidSubjectRequireMedicalAttention { get; set; }
        public string DidSubjectRequireMedicalAttentionDescription { get; set; }
        public LocalDate? DateOfBirth { get; set; }
        
        public class Validator : AbstractValidator<UpdatePersonSubjectCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.FullName).AsPersonSubjectFullName();
                RuleFor(_ => _.Age).AsPersonSubjectAge();
                RuleFor(_ => _.Gender).AsPersonSubjectGender();
                RuleFor(_ => _.Race).AsPersonSubjectRace();
                RuleFor(_ => _.SuspectedUse).AsPersonSubjectSuspectedUse();
                RuleFor(_ => _.DidSubjectRequireMedicalAttentionDescription).AsYesNoDescription();
            }

        }

        public class Handler : IRequestHandler<UpdatePersonSubjectCommand> {

            private readonly DbSet<User> _users;
            private readonly DbSet<SubjectPerson> _subjectPersons;
            private readonly IUserService _userService;

            public Handler(
                DbSet<User> users,
                DbSet<SubjectPerson> subjectPersons,
                IUserService userService) {
                
                _users = users;
                _subjectPersons = subjectPersons;
                _userService = userService;
            }

            public async Task<Unit> Handle(UpdatePersonSubjectCommand request, CancellationToken cancellationToken) {

                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _subjectPersons.ThrowIfSubjectPersonDoesNotExist(request.IncidentId, request.SubjectId,
                    cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var subjectPerson = await _subjectPersons.FirstOrDefaultAsync(
                    _ => _.IncidentId.Equals(request.IncidentId) && _.SubjectId.Equals(request.SubjectId),
                    cancellationToken);

                
                subjectPerson.FullName = request.FullName;
                subjectPerson.Age = request.Age;
                subjectPerson.Gender = request.Gender;
                subjectPerson.Race = request.Race;
                subjectPerson.SuspectedUse = request.SuspectedUse;
                subjectPerson.WasSubjectInjured = request.WasSubjectInjured;
                subjectPerson.DidSubjectRequireMedicalAttention = request.DidSubjectRequireMedicalAttention;
                subjectPerson.DidSubjectRequireMedicalAttentionDescription =
                    request.DidSubjectRequireMedicalAttentionDescription;
                subjectPerson.DateOfBirth = request.DateOfBirth?.ToDateTimeUnspecified();

                if (subjectPerson.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    subjectPerson.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    subjectPerson.TrainingApproverId = request.SubmitterId;
                } else {
                    subjectPerson.SubmitterId = request.SubmitterId;
                }

                return Unit.Value;
                
            }

        }

    }

}