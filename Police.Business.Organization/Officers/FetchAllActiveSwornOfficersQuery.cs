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
    public class FetchAllActiveSwornOfficersQuery : IRequest<IEnumerable<OfficerInfo>> {

        public class Handler : IRequestHandler<FetchAllActiveSwornOfficersQuery, IEnumerable<OfficerInfo>> {

            private readonly DbSet<Officer> _officers;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Officer> officers,
                IMapper mapper) {
                _officers = officers;
                _mapper = mapper;
            }

            public async Task<IEnumerable<OfficerInfo>> Handle(FetchAllActiveSwornOfficersQuery request,
                CancellationToken cancellationToken) =>
                await _officers.AsNoTracking().Include(_ => _.User)
                    .ProjectTo<OfficerInfo>(_mapper.ConfigurationProvider)
                    .Where(_ => (_.IsSwornOfficer ?? false) && (_.IsCurrentlyEmployed ?? false))
                    .ToListAsync(cancellationToken);

        }

    }

}