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
    public class AttachPersonSubjectCommand : IRequest<Guid> {

        public Guid IncidentId { get; set; }
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

        public class Validator : AbstractValidator<AttachPersonSubjectCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.FullName).AsPersonSubjectFullName();
                RuleFor(_ => _.Age).AsPersonSubjectAge();
                RuleFor(_ => _.Gender).AsPersonSubjectGender();
                RuleFor(_ => _.Race).AsPersonSubjectRace();
                RuleFor(_ => _.SuspectedUse).AsPersonSubjectSuspectedUse();
                RuleFor(_ => _.DidSubjectRequireMedicalAttentionDescription).AsYesNoDescription();
            }

        }

        public class Handler : IRequestHandler<AttachPersonSubjectCommand, Guid> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<User> _users;
            private readonly DbSet<Subject> _subjects;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<User> users,
                DbSet<Subject> subjects,
                IUserService userService) {
                _incidents = incidents;
                _users = users;
                _subjects = subjects;
                _userService = userService;
            }

            public async Task<Guid> Handle(AttachPersonSubjectCommand request, CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var subjectPerson = new SubjectPerson {
                    SubjectId = Guid.NewGuid(),
                    IncidentId = request.IncidentId,
                    SubmitterId = request.SubmitterId,
                    ApprovalStatus = ApprovalStatus.Created,
                    FullName = request.FullName,
                    Age = request.Age,
                    Gender = request.Gender,
                    Race = request.Race,
                    SuspectedUse = request.SuspectedUse,
                    WasSubjectInjured = request.WasSubjectInjured,
                    DidSubjectRequireMedicalAttention = request.DidSubjectRequireMedicalAttention,
                    DidSubjectRequireMedicalAttentionDescription = request.DidSubjectRequireMedicalAttentionDescription,
                    DateOfBirth = request.DateOfBirth?.ToDateTimeUnspecified()
                };

                if (subjectPerson.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    subjectPerson.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    subjectPerson.TrainingApproverId = request.SubmitterId;
                }

                _subjects.Add(subjectPerson);

                return subjectPerson.SubjectId;

            }

        }

    }

}