using System;

namespace Police.Business.ResistanceResponse.Approvals {

    public interface IApprovableEntity {

        ApprovalStatus ApprovalStatus { get; set; }
        Guid SubmitterId { get; set; }
        Guid? SupervisorApproverId { get; set; }
        Guid? TrainingApproverId { get; set; }
        string SupervisorsComments { get; set; }
        string TrainingsComments { get; set; }

    }

}