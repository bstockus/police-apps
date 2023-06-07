using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.Organization.Officers {

    public static class OfficerSpecifications {

        public static async Task ThrowIfOfficerDoesNotExist(this DbSet<Officer> officers, Guid officerId,
            CancellationToken cancellationToken) {

            if (!await officers.AsNoTracking().AnyAsync(_ => _.Id.Equals(officerId), cancellationToken)) {
                throw new Exception($"No Officer with Id '{officerId}' exists.");
            }

        }

    }

}