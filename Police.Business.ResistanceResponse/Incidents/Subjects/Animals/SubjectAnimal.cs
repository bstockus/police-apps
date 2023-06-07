using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.Animals {

    public class SubjectAnimal : Subject {

        public Species Species { get; set; }

        public class AnimalEntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, SubjectAnimal> {

            public override void Build(EntityTypeBuilder<SubjectAnimal> builder) {
                builder.FromValidator(rules => { rules.RuleFor(_ => _.Species).AsAnimalSubjectSpecies(); });
            }

        }

    }

}