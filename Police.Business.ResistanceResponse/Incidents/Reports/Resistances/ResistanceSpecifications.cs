using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Resistances {

    public static class ResistanceSpecifications {

        public static async Task ThrowIfResistanceExists(this DbSet<Resistance> resistances, Guid incidentId, Guid officerId,
            Guid subjectId, ResistanceType resistanceType, CancellationToken cancellationToken) {

            if (await resistances.AsNoTracking().AnyAsync(_ => _.IncidentId.Equals(incidentId) &&
                                                               _.OfficerId.Equals(officerId) &&
                                                               _.SubjectId.Equals(subjectId) &&
                                                               _.ResistanceType.Equals(resistanceType), cancellationToken)) {
                throw new Exception(
                    $"Report (IncidentId={incidentId}, OfficerId={officerId}, SubjectId={subjectId}) already has resistance {resistanceType}.");
            }

        }

        public static async Task ThrowIfResistanceDoesNotExist(this DbSet<Resistance> resistances, Guid incidentId, Guid officerId,
            Guid subjectId, ResistanceType resistanceType, CancellationToken cancellationToken) {

            if (!await resistances.AsNoTracking().AnyAsync(_ => _.IncidentId.Equals(incidentId) &&
                                                                _.OfficerId.Equals(officerId) &&
                                                                _.SubjectId.Equals(subjectId) &&
                                                                _.ResistanceType.Equals(resistanceType), cancellationToken)) {
                throw new Exception(
                    $"Report (IncidentId={incidentId}, OfficerId={officerId}, SubjectId={subjectId}) does not have resistance {resistanceType}.");
            }

        }

    }

}