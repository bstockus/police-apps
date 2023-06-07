using System;
using AutoMapper;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    public class IncidentOfficerListInfo : IApprovalInformation {

        public Guid IncidentId { get; set; }

        public Guid OfficerId { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public class Mapper : Profile {

            public Mapper() {
                CreateMap<IncidentOfficer, IncidentOfficerListInfo>();
            }

        }

    }

}