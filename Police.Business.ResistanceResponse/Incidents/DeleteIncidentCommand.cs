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
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class DeleteIncidentCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid ApproverId { get; set; }

        public class Validator : AbstractValidator<DeleteIncidentCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.ApproverId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<DeleteIncidentCommand> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<User> _users;
            private readonly DbSet<Report> _reports;
            private readonly DbSet<Subject> _subjects;
            private readonly DbSet<IncidentOfficer> _incidentOfficers;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<User> users,
                DbSet<Report> reports,
                DbSet<Subject> subjects,
                DbSet<IncidentOfficer> incidentOfficers,
                IUserService userService) {
                _incidents = incidents;
                _users = users;
                _reports = reports;
                _subjects = subjects;
                _incidentOfficers = incidentOfficers;
                _userService = userService;
            }

            public async Task<Unit> Handle(
                DeleteIncidentCommand request,
                CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.ApproverId, cancellationToken);

                var incident = await _incidents
                    .Include(_ => _.IncidentOfficers)
                    .Include(_ => _.Subjects)
                    .Include(_ => _.Reports)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId), cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.ApproverId);

                if (!incident.AsApprovalInformation().IsUserAllowedToDeleteIncident(user)) {
                    throw new Exception(
                        $"User {user.WindowsSid} does not have the permissions to delete Incident {incident.Id}");
                }

                _reports.RemoveRange(incident.Reports);
                _subjects.RemoveRange(incident.Subjects);
                _incidentOfficers.RemoveRange(incident.IncidentOfficers);

                _incidents.Remove(incident);


                return Unit.Value;

            }

        }

    }

}