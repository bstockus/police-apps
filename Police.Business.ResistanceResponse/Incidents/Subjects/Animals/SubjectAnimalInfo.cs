using AutoMapper;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.Animals {

    public class SubjectAnimalInfo : SubjectInfo {

        public Species Species { get; set; }

        public class AnimalMapping : Profile {

            public AnimalMapping() {
                CreateMap<SubjectAnimal, SubjectAnimalInfo>().IncludeBase<Subject, SubjectInfo>();
            }

        }

    }

}