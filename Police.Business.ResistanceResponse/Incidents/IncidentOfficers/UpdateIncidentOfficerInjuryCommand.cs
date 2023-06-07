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

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class UpdateIncidentOfficerInjuryCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid OfficerId { get; set; }

        public YesNo WasOfficerInjured { get; set; }
        public YesNo DidOfficerRequireMedicalAttention { get; set; }
        public string DidOfficerRequireMedicalAttentionDescription { get; set; }

        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<UpdateIncidentOfficerInjuryCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.OfficerId).AsEntityIdentity();
                RuleFor(_ => _.DidOfficerRequireMedicalAttentionDescription).AsYesNoDescription();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<UpdateIncidentOfficerInjuryCommand> {

            private readonly DbSet<User> _users;
            private readonly DbSet<IncidentOfficer> _incidentOfficers;
            private readonly IUserService _userService;

            public Handler(
                DbSet<User> users,
                DbSet<IncidentOfficer> incidentOfficers,
                IUserService userService) {
                _users = users;
                _incidentOfficers = incidentOfficers;
                _userService = userService;
            }

            public async Task<Unit> Handle(UpdateIncidentOfficerInjuryCommand request, CancellationToken cancellationToken) {

                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _incidentOfficers.ThrowIfIncidentOfficerDoesNotExist(request.IncidentId, request.OfficerId,
                    cancellationToken);

                var incidentOfficer = await _incidentOfficers.FirstOrDefaultAsync(
                    _ => _.IncidentId.Equals(request.IncidentId) && _.OfficerId.Equals(request.OfficerId),
                    cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                incidentOfficer.WasOfficerInjured = request.WasOfficerInjured;
                incidentOfficer.DidOfficerRequireMedicalAttention = request.DidOfficerRequireMedicalAttention;
                incidentOfficer.DidOfficerRequireMedicalAttentionDescription =
                    request.DidOfficerRequireMedicalAttentionDescription;

                if (incidentOfficer.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    incidentOfficer.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    incidentOfficer.TrainingApproverId = request.SubmitterId;
                } else {
                    incidentOfficer.SubmitterId = request.SubmitterId;
                }

                return Unit.Value;

            }

        }

    }

}