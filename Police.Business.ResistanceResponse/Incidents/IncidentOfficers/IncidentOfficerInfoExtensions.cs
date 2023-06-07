using System.Linq;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    public static class IncidentOfficerInfoExtensions {

        public static bool CanUserSubmitForOfficersOtherThanThemselves(
            this UserInformation user) =>
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsTraining);

    }

}