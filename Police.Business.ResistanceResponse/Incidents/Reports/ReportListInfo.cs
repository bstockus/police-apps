using System;
using AutoMapper;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    public class ReportListInfo : IApprovalInformation {

        public Guid IncidentId { get; set; }

        public Guid OfficerId { get; set; }

        public Guid SubjectId { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Report, ReportListInfo>();
            }

        }

    }

}