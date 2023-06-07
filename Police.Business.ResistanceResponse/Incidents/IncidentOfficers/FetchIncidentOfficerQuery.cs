using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    [LogRequest]
    public class FetchIncidentOfficerQuery : IRequest<IncidentOfficerInfo> {

        public Guid IncidentId { get; }
        public Guid OfficerId { get; }

        public FetchIncidentOfficerQuery(Guid incidentId, Guid officerId) {
            IncidentId = incidentId;
            OfficerId = officerId;
        }

        public class Handler : IRequestHandler<FetchIncidentOfficerQuery, IncidentOfficerInfo> {

            private readonly DbSet<IncidentOfficer> _incidentOfficers;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<IncidentOfficer> incidentOfficers,
                IMapper mapper) {
                _incidentOfficers = incidentOfficers;
                _mapper = mapper;
            }

            public async Task<IncidentOfficerInfo> Handle(FetchIncidentOfficerQuery request,
                CancellationToken cancellationToken) =>
                await _incidentOfficers.AsNoTracking().Include(_ => _.Officer).ProjectTo<IncidentOfficerInfo>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(
                        _ => _.IncidentId.Equals(request.IncidentId) && _.OfficerId.Equals(request.OfficerId),
                        cancellationToken);

        }

    }

}