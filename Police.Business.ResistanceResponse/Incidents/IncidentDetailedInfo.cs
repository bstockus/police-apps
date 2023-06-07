using System;
using System.Collections.Generic;
using AutoMapper;
using NodaTime;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Subjects;

namespace Police.Business.ResistanceResponse.Incidents {

    public class IncidentDetailedInfo : IIncidentApprovalInformation, IApprovalUserCommentsInformation {

        public Guid Id { get; set; }

        public LocalDateTime IncidentDateAndTime { get; set; }
        public string IncidentCaseNumber { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public string SupervisorsComments { get; set; }
        public string TrainingsComments { get; set; }

        public IEnumerable<IncidentOfficerInfo> IncidentOfficers { get; set; }
        public IEnumerable<SubjectInfo> Subjects { get; set; }
        public IEnumerable<ReportInfo> Reports { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Incident, IncidentDetailedInfo>();
            }

        }

        public IEnumerable<IApprovalInformation> IncidentOfficerApprovalInformations => IncidentOfficers;
        public IEnumerable<IApprovalInformation> SubjectApprovalInformations => Subjects;
        public IEnumerable<IApprovalInformation> ReportApprovalInformations => Reports;

        public string SubmitterUserName { get; set; }
        public string SubmitterEmailAddress { get; set; }
        public string SupervisorApproverUserName { get; set; }
        public string SupervisorApproverEmailAddress { get; set; }
        public string TrainingApproverUserName { get; set; }
        public string TrainingApproverEmailAddress { get; set; }

    }

}