using System;
using System.Linq;
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
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class SubmitIncidentForApprovalCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<SubmitIncidentForApprovalCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<SubmitIncidentForApprovalCommand> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<User> _users;
            private readonly IUserService _userService;
            private readonly IEmailNotificationsManager _emailNotificationsManager;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<User> users,
                IUserService userService,
                IEmailNotificationsManager emailNotificationsManager) {

                _incidents = incidents;
                _users = users;
                _userService = userService;
                _emailNotificationsManager = emailNotificationsManager;
            }

            public async Task<Unit> Handle(SubmitIncidentForApprovalCommand request,
                CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                var incident = await _incidents.Include(_ => _.IncidentOfficers).Include(_ => _.Subjects)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Responses).ThenInclude(_ => _.TaserUsageAddendum)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Officer)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId), cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                //1. Incident
                if (incident.AsApprovalInformation().IsUserAllowedToSubmitAsOfficer(user)) {

                    incident.GetApprovalStatusStateMachine().Fire(ApprovableEntityActions.OfficerSubmit);

                }

                //2. IncidentOfficers
                foreach (var incidentOfficer in incident.IncidentOfficers.Where(_ =>
                    ApprovableEntityExtensions.AsApprovalInformation(_).IsUserAllowedToSubmitAsOfficer(user))) {

                    incidentOfficer.GetApprovalStatusStateMachine().Fire(ApprovableEntityActions.OfficerSubmit);

                }

                //3. Subjects
                foreach (var subject in incident.Subjects.Where(_ =>
                    _.AsApprovalInformation().IsUserAllowedToSubmitAsOfficer(user))) {

                    subject.GetApprovalStatusStateMachine().Fire(ApprovableEntityActions.OfficerSubmit);

                }

                //4. Reports
                foreach (var report in incident.Reports.Where(_ =>
                    _.AsApprovalInformation().IsUserAllowedToSubmitAsOfficer(user))) {

                    report.GetApprovalStatusStateMachine().Fire(ApprovableEntityActions.OfficerSubmit);

                    if (report.Responses.Any(_ => _.ResponseType.Equals(ResponseType.Taser))) {
                        await _emailNotificationsManager.TaserSubmitNotification(request, incident, report, user);
                    }

                }

                await _emailNotificationsManager.OfficerSubmitNotification(
                    request, incident, user);

                return Unit.Value;

            }

        }

    }

}