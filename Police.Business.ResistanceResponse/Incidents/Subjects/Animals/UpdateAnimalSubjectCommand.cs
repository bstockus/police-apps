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
    public class UpdateAnimalSubjectCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid SubmitterId { get; set; }
        public Species Species { get; set; }
        
        public class Validator : AbstractValidator<UpdateAnimalSubjectCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.Species).AsAnimalSubjectSpecies();
            }

        }
        
        public class Handler : IRequestHandler<UpdateAnimalSubjectCommand> {

            private readonly DbSet<User> _users;
            private readonly DbSet<SubjectAnimal> _subjectAnimals;
            private readonly IUserService _userService;

            public Handler(
                DbSet<User> users,
                DbSet<SubjectAnimal> subjectAnimals,
                IUserService userService) {
                
                _users = users;
                _subjectAnimals = subjectAnimals;
                _userService = userService;
            }

            public async Task<Unit> Handle(UpdateAnimalSubjectCommand request, CancellationToken cancellationToken) {

                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _subjectAnimals.ThrowIfSubjectAnimalDoesNotExist(request.IncidentId, request.SubjectId,
                    cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var subjectAnimal = await _subjectAnimals.FirstOrDefaultAsync(
                    _ => _.IncidentId.Equals(request.IncidentId) && _.SubjectId.Equals(request.SubjectId),
                    cancellationToken);

                
                subjectAnimal.Species = request.Species;

                if (subjectAnimal.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    subjectAnimal.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    subjectAnimal.TrainingApproverId = request.SubmitterId;
                } else {
                    subjectAnimal.SubmitterId = request.SubmitterId;
                }

                return Unit.Value;

            }

        }

    }

}