using System;
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
    public class FetchOfficerQuery : IRequest<OfficerInfo> {

        public Guid OfficerId { get; }

        public FetchOfficerQuery(Guid officerId) {
            OfficerId = officerId;
        }

        public class Handler : IRequestHandler<FetchOfficerQuery, OfficerInfo> {

            private readonly DbSet<Officer> _officers;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Officer> officers,
                IMapper mapper) {
                _officers = officers;
                _mapper = mapper;
            }

            public async Task<OfficerInfo> Handle(FetchOfficerQuery request, CancellationToken cancellationToken) =>
                await _officers.AsNoTracking().Include(_ => _.User)
                    .ProjectTo<OfficerInfo>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.OfficerId), cancellationToken);

        }

    }

}