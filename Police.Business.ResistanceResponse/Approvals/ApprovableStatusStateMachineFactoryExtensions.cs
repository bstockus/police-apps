namespace Police.Business.ResistanceResponse.Approvals {

    public static class ApprovableStatusStateMachineFactoryExtensions {

        public static ApprovalStatusStateMachine GetApprovalStatusStateMachine(
            this IApprovableEntity approvableEntity) => new(
            () => approvableEntity.ApprovalStatus,
            _ => approvableEntity.ApprovalStatus = _);

        public static ApprovalStatusStateMachine GetApprovalStatusStateMachine(
            this IApprovalInformation approvalInformation) => new(
            approvalInformation.ApprovalStatus);

    }

}