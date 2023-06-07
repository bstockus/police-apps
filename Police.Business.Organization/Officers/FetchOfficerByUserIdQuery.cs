using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.Organization.Officers {

    [LogRequest]
    public class FetchOfficerByUserIdQuery : IRequest<OfficerInfo> {

        public Guid UserId { get; }

        public FetchOfficerByUserIdQuery(Guid userId) {
            UserId = userId;
        }

        public class Handler : IRequestHandler<FetchOfficerByUserIdQuery, OfficerInfo> {

            private readonly DbSet<Officer> _officers;
            private readonly IMapper _mapper;


            public Handler(
                DbSet<Officer> officers,
                IMapper mapper) {

                _officers = officers;
                _mapper = mapper;
            }

            public async Task<OfficerInfo> Handle(FetchOfficerByUserIdQuery request,
                CancellationToken cancellationToken) =>
                await _officers.AsNoTracking().Include(_ => _.User)
                    .ProjectTo<OfficerInfo>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(
                        _ => _.UserId.HasValue && _.UserId.Value.Equals(request.UserId), cancellationToken);

        }

    }

}