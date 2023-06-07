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
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class RemoveReportCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid OfficerId { get; set; }

        public class Validator : AbstractValidator<RemoveReportCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
                RuleFor(_ => _.OfficerId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<RemoveReportCommand> {

            private readonly DbSet<Report> _reports;
            private readonly DbSet<Resistance> _resistances;
            private readonly DbSet<Response> _responses;

            public Handler(
                DbSet<Report> reports,
                DbSet<Resistance> resistances,
                DbSet<Response> responses) {

                _reports = reports;
                _resistances = resistances;
                _responses = responses;
            }

            public async Task<Unit> Handle(RemoveReportCommand request, CancellationToken cancellationToken) {

                await _reports.ThrowIfReportDoesNotExist(request.IncidentId, request.OfficerId, request.SubjectId,
                    cancellationToken);

                var report =
                    await _reports
                        .Include(_ => _.Resistances)
                        .Include(_ => _.Responses)
                        .FirstOrDefaultAsync(_ =>
                                _.IncidentId.Equals(request.IncidentId) &&
                                _.SubjectId.Equals(request.SubjectId) &&
                                _.OfficerId.Equals(request.OfficerId),
                            cancellationToken);

                _responses.RemoveRange(report.Responses);
                _resistances.RemoveRange(report.Resistances);
                _reports.Remove(report);

                return Unit.Value;

            }

        }

    }

}