using System;
using Stateless;

namespace Police.Business.ResistanceResponse.Approvals {

    public class ApprovalStatusStateMachine : StateMachine<ApprovalStatus, ApprovableEntityActions> {

        public ApprovalStatusStateMachine(Func<ApprovalStatus> stateAccessor, Action<ApprovalStatus> stateMutator) :
            base(stateAccessor, stateMutator) {

            ConfigureStateMachine(this);
        }

        public ApprovalStatusStateMachine(ApprovalStatus initialState) : base(initialState) {

            ConfigureStateMachine(this);
        }

        private static void ConfigureStateMachine(ApprovalStatusStateMachine approvalStatusStateMachine) {

            approvalStatusStateMachine.Configure(ApprovalStatus.Created)
                .Permit(ApprovableEntityActions.OfficerSubmit, ApprovalStatus.Submitted)
                .Permit(ApprovableEntityActions.TrainingApprove, ApprovalStatus.ApprovedByTraining);

            approvalStatusStateMachine.Configure(ApprovalStatus.Submitted)
                .Permit(ApprovableEntityActions.SupervisorApprove, ApprovalStatus.ApprovedBySupervisor)
                .Permit(ApprovableEntityActions.SupervisorReject, ApprovalStatus.RejectedBySupervisor)
                .Permit(ApprovableEntityActions.TrainingApprove, ApprovalStatus.ApprovedByTraining)
                .Permit(ApprovableEntityActions.TrainingReject, ApprovalStatus.RejectedByTraining);

            approvalStatusStateMachine.Configure(ApprovalStatus.RejectedBySupervisor)
                .Permit(ApprovableEntityActions.OfficerSubmit, ApprovalStatus.Submitted)
                .Permit(ApprovableEntityActions.TrainingApprove, ApprovalStatus.ApprovedByTraining);

            approvalStatusStateMachine.Configure(ApprovalStatus.ApprovedBySupervisor)
                .Permit(ApprovableEntityActions.TrainingApprove, ApprovalStatus.ApprovedByTraining)
                .Permit(ApprovableEntityActions.TrainingReject, ApprovalStatus.RejectedByTraining);

            approvalStatusStateMachine.Configure(ApprovalStatus.RejectedByTraining)
                .Permit(ApprovableEntityActions.OfficerSubmit, ApprovalStatus.ApprovedBySupervisor);

        }

    }

}