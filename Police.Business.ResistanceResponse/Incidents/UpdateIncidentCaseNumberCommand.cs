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
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class UpdateIncidentCaseNumberCommand : IRequest {
        
        public string NewIncidentCaseNumber { get; set; }
        public Guid IncidentId { get; set; }
        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<UpdateIncidentCaseNumberCommand> {

            public Validator() {
                RuleFor(_ => _.NewIncidentCaseNumber).AsIncidentCaseNumber();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<UpdateIncidentCaseNumberCommand> {

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

            public async Task<Unit> Handle(
                UpdateIncidentCaseNumberCommand request,
                CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                await _incidents.ThrowIfIncidentWithCaseNumberExists(request.NewIncidentCaseNumber, cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);
                var incident = await _incidents.FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId),
                    cancellationToken: cancellationToken);

                incident.IncidentCaseNumber = request.NewIncidentCaseNumber;

                return Unit.Value;
            }

        }

    }

}