using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.Organization.Officers {

    [LogRequest]
    public class FetchAllOfficersWithActiveAndSwornFiltersQuery : IRequest<IEnumerable<OfficerInfo>> {

        public bool OnlyCurrentlyEmployed { get; }
        public bool OnlySworn { get; }

        public FetchAllOfficersWithActiveAndSwornFiltersQuery(
            bool onlyCurrentlyEmployed,
            bool onlySworn) {
            OnlyCurrentlyEmployed = onlyCurrentlyEmployed;
            OnlySworn = onlySworn;
        }

        public class Handler : IRequestHandler<FetchAllOfficersWithActiveAndSwornFiltersQuery, IEnumerable<OfficerInfo>> {

            private readonly DbSet<Officer> _officers;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Officer> officers,
                IMapper mapper) {
                _officers = officers;
                _mapper = mapper;
            }

            public async Task<IEnumerable<OfficerInfo>> Handle(FetchAllOfficersWithActiveAndSwornFiltersQuery request,
                CancellationToken cancellationToken) =>
                await _officers
                    .AsNoTracking()
                    .Include(_ => _.User)
                    .Where(_ => ((_.IsCurrentlyEmployed ?? false) && request.OnlyCurrentlyEmployed || !request.OnlyCurrentlyEmployed) &&
                                ((_.IsSwornOfficer ?? false) && request.OnlySworn || !request.OnlySworn))
                    .ProjectTo<OfficerInfo>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        }

    }

}