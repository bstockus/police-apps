using System;

namespace Police.Business.ResistanceResponse.Approvals {

    public class ApprovalInformation : IApprovalInformation {

        public ApprovalStatus ApprovalStatus { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }

    }

}