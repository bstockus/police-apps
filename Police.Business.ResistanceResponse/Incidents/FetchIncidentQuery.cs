using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchIncidentQuery : IRequest<IncidentInfo> {

        public Guid IncidentId { get; }

        public FetchIncidentQuery(Guid incidentId) {
            IncidentId = incidentId;
        }

        public class Handler : IRequestHandler<FetchIncidentQuery, IncidentInfo> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {
                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IncidentInfo> Handle(FetchIncidentQuery request, CancellationToken cancellationToken) =>
                await _incidents.AsNoTracking().ProjectTo<IncidentInfo>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId), cancellationToken);

        }

    }

}