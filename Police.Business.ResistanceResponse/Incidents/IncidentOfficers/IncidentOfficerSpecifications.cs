using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    public static class IncidentOfficerSpecifications {

        public static async Task ThrowIfIncidentOfficerExists(this DbSet<IncidentOfficer> incidentOfficers,
            Guid incidentId, Guid officerId, CancellationToken cancellationToken) {

            if (await incidentOfficers.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.OfficerId.Equals(officerId), cancellationToken)) {

                throw new Exception(
                    $"Officer with Id '{officerId}' is already attached to incident with Id '{incidentId}'");

            }

        }

        public static async Task ThrowIfIncidentOfficerDoesNotExist(this DbSet<IncidentOfficer> incidentOfficers,
            Guid incidentId, Guid officerId, CancellationToken cancellationToken) {

            if (!await incidentOfficers.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.OfficerId.Equals(officerId), cancellationToken)) {

                throw new Exception(
                    $"Officer with Id '{officerId}' is not attached to incident with Id '{incidentId}'");

            }

        }

    }

}