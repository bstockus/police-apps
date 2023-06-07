using System;
using System.Collections.Generic;
using AutoMapper;
using NodaTime;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Subjects;

namespace Police.Business.ResistanceResponse.Incidents {

    public class IncidentListInfo : IIncidentApprovalInformation {

        public Guid Id { get; set; }

        public LocalDateTime IncidentDateAndTime { get; set; }
        public string IncidentCaseNumber { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

        public IEnumerable<IncidentOfficerListInfo> IncidentOfficers { get; set; }
        public IEnumerable<SubjectListInfo> Subjects { get; set; }
        public IEnumerable<ReportListInfo> Reports { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Incident, IncidentListInfo>();
            }

        }

        public IEnumerable<IApprovalInformation> IncidentOfficerApprovalInformations => IncidentOfficers;
        public IEnumerable<IApprovalInformation> SubjectApprovalInformations => Subjects;
        public IEnumerable<IApprovalInformation> ReportApprovalInformations => Reports;

    }

}