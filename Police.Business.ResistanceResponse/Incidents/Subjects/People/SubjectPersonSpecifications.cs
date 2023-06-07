using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    public static class SubjectPersonSpecifications {

        public static async Task ThrowIfSubjectPersonDoesNotExist(this DbSet<SubjectPerson> subjectPersons,
            Guid incidentId, Guid subjectId, CancellationToken cancellationToken) {

            if (!await subjectPersons.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.SubjectId.Equals(subjectId), cancellationToken)) {

                throw new Exception($"No Person Subject with Id {subjectId} exists.");
            }

        }

    }

}