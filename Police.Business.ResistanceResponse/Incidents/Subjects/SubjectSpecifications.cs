using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Subjects {

    public static class SubjectSpecifications {

        public static async Task ThrowIfSubjectDoesNotExist(this DbSet<Subject> subjects, Guid incidentId,
            Guid subjectId, CancellationToken cancellationToken) {

            if (!await subjects.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.SubjectId.Equals(subjectId), cancellationToken)) {
                throw new Exception($"No subject with Id {subjectId} exists.");
            }

        }

        public static async Task ThrowIfSubjectExists(this DbSet<Subject> subjects, Guid incidentId,
            Guid subjectId, CancellationToken cancellationToken) {

            if (await subjects.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.SubjectId.Equals(subjectId), cancellationToken)) {
                throw new Exception($"Subject with Id {subjectId} exists.");
            }

        }

    }

}