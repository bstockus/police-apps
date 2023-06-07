using System.Threading.Tasks;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    public interface IEmailNotificationsManager {

        Task OfficerSubmitNotification(SubmitIncidentForApprovalCommand submitIncidentForApprovalCommand,
            Incident incident, UserInformation userInformation);

        Task TaserSubmitNotification(SubmitIncidentForApprovalCommand submitIncidentForApprovalCommand,
            Incident incident, Report report, UserInformation userInformation);

        Task ApproveOrRejectNotification(ApproveOrRejectIncidentCommand approveOrRejectIncidentCommand,
            Incident incident, UserInformation userInformation);

    }

}