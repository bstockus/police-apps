using AutoMapper;

namespace Police.Business.ResistanceResponse.Incidents.Subjects {

    public class SubjectListInfo : SubjectInfo {

        public class ListMapping : Profile {

            public ListMapping() {
                CreateMap<Subject, SubjectListInfo>().IncludeBase<Subject, SubjectInfo>();
            }

        }

    }

}