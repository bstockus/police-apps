using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest]
    public class FetchIncidentByCaseNumberQuery : IRequest<IncidentInfo> {

        public string CaseNumber { get; }

        public FetchIncidentByCaseNumberQuery(string caseNumber) {
            CaseNumber = caseNumber;
        }

        public class Handler : IRequestHandler<FetchIncidentByCaseNumberQuery, IncidentInfo> {

            private readonly DbSet<Incident> _incidents;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Incident> incidents,
                IMapper mapper) {
                _incidents = incidents;
                _mapper = mapper;
            }

            public async Task<IncidentInfo> Handle(FetchIncidentByCaseNumberQuery request,
                CancellationToken cancellationToken) =>
                await _incidents.AsNoTracking().ProjectTo<IncidentInfo>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(_ => _.IncidentCaseNumber.Equals(request.CaseNumber), cancellationToken);

        }

    }

}