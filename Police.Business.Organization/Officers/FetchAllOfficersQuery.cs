using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.Organization.Officers {

    [LogRequest]
    public class FetchAllOfficersQuery : IRequest<IEnumerable<OfficerInfo>> {

        public class Handler : IRequestHandler<FetchAllOfficersQuery, IEnumerable<OfficerInfo>> {

            private readonly DbSet<Officer> _officers;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Officer> officers,
                IMapper mapper) {
                _officers = officers;
                _mapper = mapper;
            }

            public async Task<IEnumerable<OfficerInfo>> Handle(FetchAllOfficersQuery request,
                CancellationToken cancellationToken) =>
                await _officers.AsNoTracking().Include(_ => _.User)
                    .ProjectTo<OfficerInfo>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        }

    }

}