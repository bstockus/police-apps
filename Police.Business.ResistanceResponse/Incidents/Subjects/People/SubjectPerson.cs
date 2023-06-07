using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    public class SubjectPerson : Subject {

        public string FullName { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        public SuspectedUse SuspectedUse { get; set; }
        public YesNo WasSubjectInjured { get; set; }
        public YesNo DidSubjectRequireMedicalAttention { get; set; }
        public string DidSubjectRequireMedicalAttentionDescription { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public class PersonEntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, SubjectPerson> {

            public override void Build(EntityTypeBuilder<SubjectPerson> builder) {
                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.FullName).AsPersonSubjectFullName();
                    //rules.RuleFor(_ => _.Age).AsPersonSubjectAge();
                    rules.RuleFor(_ => _.Gender).AsPersonSubjectGender();
                    rules.RuleFor(_ => _.Race).AsPersonSubjectRace();
                    rules.RuleFor(_ => _.SuspectedUse).AsPersonSubjectSuspectedUse();
                    rules.RuleFor(_ => _.DidSubjectRequireMedicalAttentionDescription).AsYesNoDescription();
                });
            }

        }

    }

}