using System.Linq;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Approvals {

    public static class ApprovalInformationExtensions {

        public static bool IsUserAllowedToMakeChanges(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            ((approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Created) ||
              approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedBySupervisor) ||
              approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedByTraining)) &&
             user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToSubmit) &&
             user.UserId.Equals(approvalInformation.SubmitterId)) ||
            (approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Submitted) &&
             user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsSupervisor)) ||
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsTraining);

        public static bool IsUserAllowedToView(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            ((approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Created) ||
              approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedBySupervisor) ||
              approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedByTraining)) &&
             user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToSubmit) &&
             user.UserId.Equals(approvalInformation.SubmitterId)) ||
            (approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Submitted) &&
             user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsSupervisor)) ||
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsTraining);

        public static bool IsUserAllowedToDeleteIncident(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            !approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Converted) &&
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToDeleteIncidents);

        public static bool IsUserAllowedToUpdateIncident(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            !approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Converted) &&
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToUpdateIncidents);

        public static bool IsUserAllowedToMakeChangesWithoutApproval(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            user.EffectivePermissions.Contains(ResistanceResponsePermissions.AllowedToApproveAsTraining);

        public static bool IsUserAllowedToSubmitAsOfficer(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.GetApprovalStatusStateMachine().CanFire(ApprovableEntityActions.OfficerSubmit) &&
            user.EffectivePermissions.Any(_ => _.Equals(ResistanceResponsePermissions.AllowedToSubmit)) &&
            user.UserId.Equals(approvalInformation.SubmitterId);

        public static bool IsUserAllowedToOriginalSubmitAsOfficer(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            !approvalInformation.IsRejected() &&
            approvalInformation.IsUserAllowedToSubmitAsOfficer(user);

        public static bool IsUserAllowedToReSubmitAsOfficer(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.IsRejected() &&
            approvalInformation.IsUserAllowedToSubmitAsOfficer(user);

        public static bool IsUserAllowedToApproveAsSupervisor(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.GetApprovalStatusStateMachine().CanFire(ApprovableEntityActions.SupervisorApprove) &&
            user.EffectivePermissions.Any(_ => _.Equals(ResistanceResponsePermissions.AllowedToApproveAsSupervisor));

        public static bool IsUserAllowedToRejectAsSupervisor(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.GetApprovalStatusStateMachine().CanFire(ApprovableEntityActions.SupervisorReject) &&
            user.EffectivePermissions.Any(_ => _.Equals(ResistanceResponsePermissions.AllowedToApproveAsSupervisor));

        public static bool IsUserAllowedToApproveOrRejectAsSupervisor(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.IsUserAllowedToApproveAsSupervisor(user) ||
            approvalInformation.IsUserAllowedToRejectAsSupervisor(user);

        public static bool IsUserAllowedToApproveAsTraining(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.GetApprovalStatusStateMachine().CanFire(ApprovableEntityActions.TrainingApprove) &&
            user.EffectivePermissions.Any(_ => _.Equals(ResistanceResponsePermissions.AllowedToApproveAsTraining));

        public static bool IsUserAllowedToRejectAsTraining(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.GetApprovalStatusStateMachine().CanFire(ApprovableEntityActions.TrainingReject) &&
            user.EffectivePermissions.Any(_ => _.Equals(ResistanceResponsePermissions.AllowedToApproveAsTraining));

        public static bool IsUserAllowedToApproveOrRejectAsTraining(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.IsUserAllowedToApproveAsTraining(user) ||
            approvalInformation.IsUserAllowedToRejectAsTraining(user);

        public static bool IsUserAllowedToApproveOrRejectAsSupervisorOrTraining(
            this IApprovalInformation approvalInformation,
            UserInformation user) =>
            approvalInformation.IsUserAllowedToApproveOrRejectAsSupervisor(user) ||
            approvalInformation.IsUserAllowedToApproveOrRejectAsTraining(user);

        public static bool IsUserAllowedToPerformActionOnEntity(
            this IApprovalInformation approvalInformation,
            UserInformation user,
            ApprovableEntityActions action) {

            switch (action) {
                case ApprovableEntityActions.OfficerSubmit:
                    return approvalInformation.IsUserAllowedToSubmitAsOfficer(user);
                case ApprovableEntityActions.SupervisorApprove:
                    return approvalInformation.IsUserAllowedToApproveAsSupervisor(user);
                case ApprovableEntityActions.SupervisorReject:
                    return approvalInformation.IsUserAllowedToRejectAsSupervisor(user);
                case ApprovableEntityActions.TrainingApprove:
                    return approvalInformation.IsUserAllowedToApproveAsTraining(user);
                case ApprovableEntityActions.TrainingReject:
                    return approvalInformation.IsUserAllowedToRejectAsTraining(user);
                default:
                    return false;
            }

        }

        public static bool IsRejected(this IApprovalInformation approvalInformation) =>
            approvalInformation.IsRejectedBySupervisor() || approvalInformation.IsRejectedByTraining();

        public static bool IsRejectedBySupervisor(this IApprovalInformation approvalInformation) =>
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedBySupervisor);

        public static bool IsRejectedByTraining(this IApprovalInformation approvalInformation) =>
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.RejectedByTraining);

        public static bool IsApproved(this IApprovalInformation approvalInformation) =>
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.ApprovedByTraining) ||
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Converted);

        public static bool IsPending(this IApprovalInformation approvalInformation) =>
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Submitted) ||
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.ApprovedBySupervisor);

        public static bool IsUnSubmitted(this IApprovalInformation approvalInformation) =>
            approvalInformation.GetApprovalStatusStateMachine().IsInState(ApprovalStatus.Created);

    }

}