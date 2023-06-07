using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchDetailedIncidentQuery : IRequest<IncidentDetailedInfo> {

        public Guid IncidentId { get; }

        public FetchDetailedIncidentQuery(Guid incidentId) {
            IncidentId = incidentId;
        }

        public class Handler : IRequestHandler<FetchDetailedIncidentQuery, IncidentDetailedInfo> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {
                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IncidentDetailedInfo> Handle(FetchDetailedIncidentQuery request,
                CancellationToken cancellationToken) =>
                _mapper.Map<IncidentDetailedInfo>(await _incidents.AsNoTracking()
                    .Include(_ => _.Submitter)
                    .Include(_ => _.SupervisorApprover)
                    .Include(_ => _.TrainingApprover)
                    .Include(_ => _.IncidentOfficers).ThenInclude(_ => _.Officer)
                    .Include(_ => _.IncidentOfficers).ThenInclude(_ => _.Submitter)
                    .Include(_ => _.IncidentOfficers).ThenInclude(_ => _.SupervisorApprover)
                    .Include(_ => _.IncidentOfficers).ThenInclude(_ => _.TrainingApprover)
                    .Include(_ => _.Subjects)
                    .Include(_ => _.Subjects).ThenInclude(_ => _.Submitter)
                    .Include(_ => _.Subjects).ThenInclude(_ => _.SupervisorApprover)
                    .Include(_ => _.Subjects).ThenInclude(_ => _.TrainingApprover)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Submitter)
                    .Include(_ => _.Reports).ThenInclude(_ => _.SupervisorApprover)
                    .Include(_ => _.Reports).ThenInclude(_ => _.TrainingApprover)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Officer)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Resistances)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Responses)
                    .ThenInclude(_ => _.FireArmDeadlyForceAddendum)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Responses).ThenInclude(_ => _.OtherDeadlyForceAddendum)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Responses).ThenInclude(_ => _.PitUsageAddendum)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Responses).ThenInclude(_ => _.TaserUsageAddendum)
                    .ThenInclude(_ => _.BodyUsageLocations)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Subject)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Officer)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId), cancellationToken));

        }

    }

}