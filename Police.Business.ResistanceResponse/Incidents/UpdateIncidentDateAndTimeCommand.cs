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
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class UpdateIncidentDateAndTimeCommand : IRequest {

        public LocalDateTime NewIncidentDateAndTime { get; set; }
        public Guid IncidentId { get; set; }
        public Guid SubmitterId { get; set; }

        public class Validator : AbstractValidator<UpdateIncidentDateAndTimeCommand> {

            public Validator() {
                RuleFor(_ => _.NewIncidentDateAndTime).AsIncidentDateAndTime();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<UpdateIncidentDateAndTimeCommand> {

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
                UpdateIncidentDateAndTimeCommand request,
                CancellationToken cancellationToken) {
                
                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);
                var incident = await _incidents.FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId),
                    cancellationToken: cancellationToken);

                incident.IncidentDateAndTime = request.NewIncidentDateAndTime.ToDateTimeUnspecified();

                return Unit.Value;
            }

        }

    }

}