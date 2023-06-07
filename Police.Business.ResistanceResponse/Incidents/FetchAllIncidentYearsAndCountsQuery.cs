using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchAllIncidentYearsAndCountsQuery : IRequest<IEnumerable<FetchAllIncidentYearsAndCountsQuery.IncidentYearsAndCounts>> {

        public record IncidentYearsAndCounts(int Year, int Count);

        public class Handler : IRequestHandler<FetchAllIncidentYearsAndCountsQuery, IEnumerable<IncidentYearsAndCounts>> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {
                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IEnumerable<IncidentYearsAndCounts>> Handle(
                FetchAllIncidentYearsAndCountsQuery request,
                CancellationToken cancellationToken) =>
                await _incidents.AsNoTracking()
                    .GroupBy(_ => _.IncidentDateAndTime.Year)
                    .Select(_ => new IncidentYearsAndCounts(_.Key, _.Count()))
                    .ToListAsync(cancellationToken);

        }

    }

}