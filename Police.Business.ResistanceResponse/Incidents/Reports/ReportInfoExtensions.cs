using System.Linq;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    public static class ReportInfoExtensions {

        public static bool CanUserViewReport(this ReportInfo report, UserInformation user) =>
            (user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToSubmit) &&
             user.UserId.Equals(report.SubmitterId)) ||
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsSupervisor) ||
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsTraining) ||
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToViewAllReports);

    }

}