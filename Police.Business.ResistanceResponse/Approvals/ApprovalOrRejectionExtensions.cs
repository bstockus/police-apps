namespace Police.Business.ResistanceResponse.Approvals {

    public static class ApprovalOrRejectionExtensions {

        public static ApprovableEntityActions ToApprovalEntityAction(this ApprovalOrRejection approvalOrRejection) =>
            (ApprovableEntityActions) ((int) approvalOrRejection);

        public static bool IsApprovalEntityActionForSupervisor(this ApprovalOrRejection approvalOrRejection) =>
            approvalOrRejection.Equals(ApprovalOrRejection.SupervisorApprove) ||
            approvalOrRejection.Equals(ApprovalOrRejection.SupervisorReject);
        
    }

}