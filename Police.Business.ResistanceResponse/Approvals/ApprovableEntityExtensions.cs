namespace Police.Business.ResistanceResponse.Approvals {

    public static class ApprovableEntityExtensions {

        public static IApprovalInformation AsApprovalInformation(this IApprovableEntity approvableEntity) =>
            new ApprovalInformation {
                ApprovalStatus = approvableEntity.ApprovalStatus,
                SubmitterId = approvableEntity.SubmitterId,
                SupervisorApproverId = approvableEntity.SupervisorApproverId,
                TrainingApproverId = approvableEntity.TrainingApproverId
            };

    }

}