using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.ResistanceResponse.Approvals;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.Animals {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class AttachAnimalSubjectCommand : IRequest<Guid> {

        public Guid IncidentId { get; set; }
        public Guid SubmitterId { get; set; }
        public Species Species { get; set; }

        public class Validator : AbstractValidator<AttachAnimalSubjectCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.Species).AsAnimalSubjectSpecies();
            }

        }

        public class Handler : IRequestHandler<AttachAnimalSubjectCommand, Guid> {

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

            public async Task<Guid> Handle(AttachAnimalSubjectCommand request, CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var subjectAnimal = new SubjectAnimal {
                    SubjectId = Guid.NewGuid(),
                    IncidentId = request.IncidentId,
                    SubmitterId = request.SubmitterId,
                    ApprovalStatus = ApprovalStatus.Created,
                    Species = request.Species
                };

                if (subjectAnimal.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    subjectAnimal.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    subjectAnimal.TrainingApproverId = request.SubmitterId;
                }

                _subjects.Add(subjectAnimal);

                return subjectAnimal.SubjectId;

            }

        }

    }

}