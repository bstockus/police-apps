using System;
using System.Linq;
using Police.Business.ResistanceResponse.Approvals;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    public static class IncidentApprovalInformationExtensions {

        private static bool CanUserPerformActionOnIncidentOrItsChildren(
            IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user,
            Func<IApprovalInformation, UserInformation, bool> func) =>
            func(incidentApprovalInformation, user) ||
            incidentApprovalInformation.IncidentOfficerApprovalInformations.Any(_ => func(_, user)) ||
            incidentApprovalInformation.SubjectApprovalInformations.Any(_ => func(_, user)) ||
            incidentApprovalInformation.ReportApprovalInformations.Any(_ => func(_, user));
        

        public static bool CanUserViewIncidentOrAnyOfItsChilderen(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToView(user));

        public static bool CanUserSubmitIncidentOrAnyChildrenAsOfficer(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToSubmitAsOfficer(userInformation));

        public static bool CanUserApproveIncidentOrAnyChildrenAsSupervisor(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToApproveAsSupervisor(userInformation));

        public static bool CanUserRejectIncidentOrAnyChildrenAsSupervisor(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToRejectAsSupervisor(userInformation));

        public static bool CanUserApproveIncidentOrAnyChildrenAsTraining(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToApproveAsTraining(userInformation));

        public static bool CanUserRejectIncidentOrAnyChildrenAsTraining(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToRejectAsTraining(userInformation));

        public static bool CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisor(
            this IIncidentApprovalInformation incidentApprovalInformation, UserInformation user) =>
            incidentApprovalInformation.CanUserApproveIncidentOrAnyChildrenAsSupervisor(user) ||
            incidentApprovalInformation.CanUserRejectIncidentOrAnyChildrenAsSupervisor(user);

        public static bool CanUserApproveOrRejectIncidentOrAnyChildrenAsTraining(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            incidentApprovalInformation.CanUserApproveIncidentOrAnyChildrenAsTraining(user) ||
            incidentApprovalInformation.CanUserRejectIncidentOrAnyChildrenAsTraining(user);

        public static bool CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisorOrTraining(
            this IIncidentApprovalInformation incidentApprovalInformation, UserInformation user) =>
            incidentApprovalInformation.CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisor(user) ||
            incidentApprovalInformation.CanUserApproveOrRejectIncidentOrAnyChildrenAsTraining(user);

        public static bool CanUserOriginalSubmitIncidentOrAnyChildrenAsOfficer(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToOriginalSubmitAsOfficer(user));

        public static bool CanUserReSubmitIncidentOrAnyChildrenAsOfficer(
            this IIncidentApprovalInformation incidentApprovalInformation,
            UserInformation user) =>
            CanUserPerformActionOnIncidentOrItsChildren(incidentApprovalInformation, user,
                (information, userInformation) => information.IsUserAllowedToReSubmitAsOfficer(user));


    }

}