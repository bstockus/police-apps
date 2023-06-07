using System;
using AutoMapper;
using NodaTime;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents {

    public class IncidentInfo : IApprovalInformation {

        public Guid Id { get; set; }

        public LocalDateTime IncidentDateAndTime { get; set; }
        public string IncidentCaseNumber { get; set; }
        
        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Incident, IncidentInfo>();
            }

        }

    }

}