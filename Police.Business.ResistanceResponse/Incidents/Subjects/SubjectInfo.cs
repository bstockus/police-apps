using System;
using AutoMapper;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents.Subjects {

    public abstract class SubjectInfo : IApprovalUserCommentsInformation {

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }
        public string SupervisorsComments { get; set; }
        public string TrainingsComments { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Subject, SubjectInfo>();
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