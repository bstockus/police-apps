using System;

namespace Police.Business.ResistanceResponse.Approvals {

    public interface IApprovalInformation {

        ApprovalStatus ApprovalStatus { get; }
        Guid SubmitterId { get; }
        Guid? SupervisorApproverId { get; }
        Guid? TrainingApproverId { get; }

    }

}