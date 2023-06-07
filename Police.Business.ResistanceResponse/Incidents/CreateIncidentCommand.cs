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

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class CreateIncidentCommand : IRequest<Guid> {

        public LocalDateTime IncidentDateAndTime { get; set; }
        public string IncidentCaseNumber { get; set; }
        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<CreateIncidentCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentCaseNumber).AsIncidentCaseNumber();
                RuleFor(_ => _.IncidentDateAndTime).AsIncidentDateAndTime();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<CreateIncidentCommand, Guid> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<User> _users;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<User> users,
                IUserService userService) {
                _incidents = incidents;
                _users = users;
                _userService = userService;
            }

            public async Task<Guid> Handle(CreateIncidentCommand request, CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentWithCaseNumberExists(request.IncidentCaseNumber, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var incident = new Incident {
                    Id = Guid.NewGuid(),
                    ApprovalStatus = ApprovalStatus.Created,
                    IncidentCaseNumber = request.IncidentCaseNumber,
                    IncidentDateAndTime = request.IncidentDateAndTime.ToDateTimeUnspecified(),
                    SubmitterId = request.SubmitterId
                };

                if (incident.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    incident.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    incident.TrainingApproverId = request.SubmitterId;
                }

                _incidents.Add(incident);

                return incident.Id;

            }

        }

    }

}