namespace Police.Business.ResistanceResponse.Approvals {

    public static class ApprovalOrRejectionDataExtensions {

        public static bool IsApproval(this ApprovalOrRejectionData approvalOrRejectionData) =>
            (approvalOrRejectionData.ApprovalOrRejection == ApprovalOrRejection.SupervisorApprove ||
            approvalOrRejectionData.ApprovalOrRejection == ApprovalOrRejection.TrainingApprove);

        public static bool IsRejection(this ApprovalOrRejectionData approvalOrRejectionData) =>
            !approvalOrRejectionData.IsApproval();

        public static bool IsSupervisorApproval(this ApprovalOrRejectionData approvalOrRejectionData) =>
            (approvalOrRejectionData.ApprovalOrRejection == ApprovalOrRejection.SupervisorApprove);

    }

    public abstract class ApprovalOrRejectionData {

        public ApprovalOrRejection ApprovalOrRejection { get; set; }

        public string Comments { get; set; }

    }

}