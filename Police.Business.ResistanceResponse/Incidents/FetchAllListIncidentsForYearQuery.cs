using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchAllListIncidentsForYearQuery : IRequest<IEnumerable<IncidentListInfo>> {

        public int Year { get; }

        public FetchAllListIncidentsForYearQuery(int year) {
            Year = year;
        }

        public class Handler : IRequestHandler<FetchAllListIncidentsForYearQuery, IEnumerable<IncidentListInfo>> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {

                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IEnumerable<IncidentListInfo>> Handle(
                FetchAllListIncidentsForYearQuery request,
                CancellationToken cancellationToken) =>
                await _incidents.AsNoTracking()
                    .Where(_ => _.IncidentDateAndTime.Year.Equals(request.Year))
                    .ProjectTo<IncidentListInfo>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        }

    }

}