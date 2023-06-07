using System;
using AutoMapper;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    public class IncidentOfficerInfo : IApprovalUserCommentsInformation {

        public Guid IncidentId { get; set; }

        public Guid OfficerId { get; set; }

        public string OfficerUserName { get; set; }
        public string OfficerFirstName { get; set; }
        public string OfficerLastName { get; set; }
        public string OfficerEmailAddress { get; set; }
        public string OfficerBadgeNumber { get; set; }
        public string OfficerEmployeeNumber { get; set; }
        public string OfficerRank { get; set; }
        public string OfficerJobTitle { get; set; }
        public string OfficerAssignment { get; set; }

        public YesNo WasOfficerInjured { get; set; }
        public YesNo DidOfficerRequireMedicalAttention { get; set; }
        public string DidOfficerRequireMedicalAttentionDescription { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }
        public string SupervisorsComments { get; set; }
        public string TrainingsComments { get; set; }

        public class Mapper : Profile {

            public Mapper() {
                CreateMap<IncidentOfficer, IncidentOfficerInfo>();
            }

        }

        public string SubmitterUserName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterLastName { get; set; }
        public string SubmitterEmailAddress { get; set; }
        public string SupervisorApproverUserName { get; set; }
        public string SupervisorApproverFirstName { get; set; }
        public string SupervisorApproverLastName { get; set; }
        public string SupervisorApproverEmailAddress { get; set; }
        public string TrainingApproverUserName { get; set; }
        public string TrainingApproverFirstName { get; set; }
        public string TrainingApproverLastName { get; set; }
        public string TrainingApproverEmailAddress { get; set; }

    }

}