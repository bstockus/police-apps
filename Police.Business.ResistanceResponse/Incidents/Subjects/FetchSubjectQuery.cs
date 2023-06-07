using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Subjects {

    public class FetchSubjectQuery : IRequest<SubjectInfo> {

        public Guid IncidentId { get; }
        public Guid SubjectId { get; }

        public FetchSubjectQuery(Guid incidentId, Guid subjectId) {
            IncidentId = incidentId;
            SubjectId = subjectId;
        }

        public class Handler : IRequestHandler<FetchSubjectQuery, SubjectInfo> {

            private readonly DbSet<Subject> _subjects;
            private readonly IMapper _mapper;

            public Handler(
                DbSet<Subject> subjects,
                IMapper mapper) {

                _subjects = subjects;
                _mapper = mapper;
            }

            public async Task<SubjectInfo> Handle(FetchSubjectQuery request, CancellationToken cancellationToken) =>
                _mapper.Map<SubjectInfo>(await _subjects.AsNoTracking()
                    .FirstOrDefaultAsync(
                        _ => _.IncidentId.Equals(request.IncidentId) && _.SubjectId.Equals(request.SubjectId),
                        cancellationToken));

        }

    }

}