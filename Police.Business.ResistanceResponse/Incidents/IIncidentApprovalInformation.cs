using System.Collections.Generic;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Business.ResistanceResponse.Incidents {

    public interface IIncidentApprovalInformation : IApprovalInformation {

        IEnumerable<IApprovalInformation> IncidentOfficerApprovalInformations { get; }
        IEnumerable<IApprovalInformation> SubjectApprovalInformations { get; }
        IEnumerable<IApprovalInformation> ReportApprovalInformations { get; }

    }

}