using AutoMapper;
using NodaTime;
using Police.Business.Common;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    public class SubjectPersonInfo : SubjectInfo {

        public string FullName { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        public SuspectedUse SuspectedUse { get; set; }
        public YesNo WasSubjectInjured { get; set; }
        public YesNo DidSubjectRequireMedicalAttention { get; set; }
        public string DidSubjectRequireMedicalAttentionDescription { get; set; }
        public LocalDate? DateOfBirth { get; set; }

        public class PersonMapping : Profile {

            public PersonMapping() {
                CreateMap<SubjectPerson, SubjectPersonInfo>().IncludeBase<Subject, SubjectInfo>();
            }

        }

    }

}