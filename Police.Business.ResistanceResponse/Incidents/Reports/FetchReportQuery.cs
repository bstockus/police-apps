using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    [LogRequest]
    public class FetchReportQuery : IRequest<ReportInfo> {

        public Guid IncidentId { get; }
        public Guid SubjectId { get; }
        public Guid OfficerId { get; }

        public FetchReportQuery(
            Guid incidentId,
            Guid subjectId,
            Guid officerId) {
            IncidentId = incidentId;
            SubjectId = subjectId;
            OfficerId = officerId;
        }

        public class Handler : IRequestHandler<FetchReportQuery, ReportInfo> {

            private readonly DbSet<Report> _reports;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Report> reports,
                IMapper mapper) {

                _reports = reports;
                _mapper = mapper;
            }

            public async Task<ReportInfo> Handle(FetchReportQuery request, CancellationToken cancellationToken) =>
                _mapper.Map<ReportInfo>(await _reports.AsNoTracking()
                    .Include(_ => _.Officer)
                    .Include(_ => _.Subject)
                    .Include(_ => _.Resistances)
                    .Include(_ => _.Responses).ThenInclude(_ => _.FireArmDeadlyForceAddendum)
                    .Include(_ => _.Responses).ThenInclude(_ => _.OtherDeadlyForceAddendum)
                    .Include(_ => _.Responses).ThenInclude(_ => _.PitUsageAddendum)
                    .Include(_ => _.Responses).ThenInclude(_ => _.TaserUsageAddendum)
                    .ThenInclude(_ => _.BodyUsageLocations)
                    .FirstOrDefaultAsync(_ =>
                            _.IncidentId.Equals(request.IncidentId) &&
                            _.SubjectId.Equals(request.SubjectId) &&
                            _.OfficerId.Equals(request.OfficerId),
                        cancellationToken));

        }

    }

}