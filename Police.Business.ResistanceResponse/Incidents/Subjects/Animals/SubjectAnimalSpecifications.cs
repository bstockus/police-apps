using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.Animals {

    public static class SubjectAnimalSpecifications {

        public static async Task ThrowIfSubjectAnimalDoesNotExist(this DbSet<SubjectAnimal> subjectAnimals,
            Guid incidentId, Guid subjectId, CancellationToken cancellationToken) {

            if (!await subjectAnimals.AsNoTracking()
                .AnyAsync(_ => _.IncidentId.Equals(incidentId) && _.SubjectId.Equals(subjectId), cancellationToken)) {
                
                throw new Exception($"No Animal Subject with Id {subjectId} exists.");
            }
            
        }

    }

}