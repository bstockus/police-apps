using System.Threading;
using System.Threading.Tasks;
using Lax.Business.Bus.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.Organization.Officers {

    [LogRequest]
    public class FetchAllOfficerCountsByFiltersQuery : IRequest<FetchAllOfficerCountsByFiltersQuery.Results> {

        public record Results(
            int AllOfficersCount,
            int CurrentlyEmployedOnlyCount,
            int SwornOnlyCount,
            int CurrentlyEmployedAndSwornCount);

        public class Handler : IRequestHandler<FetchAllOfficerCountsByFiltersQuery, Results> {

            private readonly DbSet<Officer> _officers;

            public Handler(
                DbSet<Officer> officers) {
                _officers = officers;
            }

            public async Task<Results> Handle(
                FetchAllOfficerCountsByFiltersQuery request,
                CancellationToken cancellationToken) =>
                new(
                    await _officers.AsNoTracking().CountAsync(cancellationToken),
                    await _officers.AsNoTracking().CountAsync(
                        _ => _.IsCurrentlyEmployed ?? false,
                        cancellationToken),
                    await _officers.AsNoTracking().CountAsync(
                        _ => _.IsSwornOfficer ?? false,
                        cancellationToken),
                    await _officers.AsNoTracking().CountAsync(
                        _ => (_.IsCurrentlyEmployed ?? false) && (_.IsSwornOfficer ?? false),
                        cancellationToken));

        }

    }

}