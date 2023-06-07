using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents {

    public static class IncidentSpecifications {

        public static async Task ThrowIfIncidentDoesNotExist(this DbSet<Incident> incidents, Guid incidentId,
            CancellationToken cancellationToken) {
            if (!await incidents.AsNoTracking().AnyAsync(_ => _.Id.Equals(incidentId), cancellationToken)) {
                throw new Exception($"No Incident with Id '{incidentId}' exists.");
            }
        }

        public static async Task ThrowIfIncidentWithCaseNumberExists(this DbSet<Incident> incidents, string caseNumber,
            CancellationToken cancellationToken) {
            if (await incidents.AsNoTracking()
                .AnyAsync(_ => _.IncidentCaseNumber.Equals(caseNumber), cancellationToken)) {
                throw new Exception($"An incident with case number {caseNumber} already exists.");
            }
        }

    }

}