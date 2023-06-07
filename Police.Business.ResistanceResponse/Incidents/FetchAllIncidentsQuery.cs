using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchAllIncidentsQuery : IRequest<IEnumerable<IncidentInfo>> {

        public class Handler : IRequestHandler<FetchAllIncidentsQuery, IEnumerable<IncidentInfo>> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {
                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IEnumerable<IncidentInfo>> Handle(FetchAllIncidentsQuery request,
                CancellationToken cancellationToken) =>
                await _incidents.AsNoTracking().ProjectTo<IncidentInfo>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        }

    }

}