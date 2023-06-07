using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    public static class ReportSpecifications {

        public static async Task ThrowIfReportExists(this DbSet<Report> reports, Guid incidentId, Guid officerId,
            Guid subjectId, CancellationToken cancellationToken) {

            if (await reports.AsNoTracking()
                .AnyAsync(
                    _ => _.IncidentId.Equals(incidentId) && _.OfficerId.Equals(officerId) &&
                         _.SubjectId.Equals(subjectId), cancellationToken)) {

                throw new Exception(
                    $"Report with (IncidentId={incidentId}, OfficerId={officerId}, SubjectId={subjectId}) already exists.");

            }

        }

        public static async Task ThrowIfReportDoesNotExist(this DbSet<Report> reports, Guid incidentId, Guid officerId,
            Guid subjectId, CancellationToken cancellationToken) {

            if (!await reports.AsNoTracking()
                .AnyAsync(
                    _ => _.IncidentId.Equals(incidentId) && _.OfficerId.Equals(officerId) &&
                         _.SubjectId.Equals(subjectId), cancellationToken)) {

                throw new Exception(
                    $"Report with (IncidentId={incidentId}, OfficerId={officerId}, SubjectId={subjectId}) does not exist.");

            }

        }

    }

}