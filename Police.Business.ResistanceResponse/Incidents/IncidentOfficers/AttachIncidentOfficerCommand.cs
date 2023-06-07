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
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Approvals;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class AttachIncidentOfficerCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid OfficerId { get; set; }

        public YesNo WasOfficerInjured { get; set; }
        public YesNo DidOfficerRequireMedicalAttention { get; set; }
        public string DidOfficerRequireMedicalAttentionDescription { get; set; }

        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<AttachIncidentOfficerCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.OfficerId).AsEntityIdentity();
                RuleFor(_ => _.DidOfficerRequireMedicalAttentionDescription).AsYesNoDescription();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<AttachIncidentOfficerCommand> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<Officer> _officers;
            private readonly DbSet<User> _users;
            private readonly DbSet<IncidentOfficer> _incidentOfficers;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<Officer> officers,
                DbSet<User> users,
                DbSet<IncidentOfficer> incidentOfficers,
                IUserService userService) {

                _incidents = incidents;
                _officers = officers;
                _users = users;
                _incidentOfficers = incidentOfficers;
                _userService = userService;
            }

            public async Task<Unit> Handle(AttachIncidentOfficerCommand request, CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _officers.ThrowIfOfficerDoesNotExist(request.OfficerId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _incidentOfficers.ThrowIfIncidentOfficerExists(request.IncidentId, request.OfficerId,
                    cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                var incidentOfficer = new IncidentOfficer {
                    IncidentId = request.IncidentId,
                    OfficerId = request.OfficerId,
                    WasOfficerInjured = request.WasOfficerInjured,
                    DidOfficerRequireMedicalAttention = request.DidOfficerRequireMedicalAttention,
                    DidOfficerRequireMedicalAttentionDescription = request.DidOfficerRequireMedicalAttentionDescription,
                    SubmitterId = request.SubmitterId,
                    ApprovalStatus = ApprovalStatus.Created
                };

                if (incidentOfficer.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    incidentOfficer.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    incidentOfficer.TrainingApproverId = request.SubmitterId;
                }

                _incidentOfficers.Add(incidentOfficer);

                return Unit.Value;

            }

        }

    }

}