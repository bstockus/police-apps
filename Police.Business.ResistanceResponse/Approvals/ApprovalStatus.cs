using System.ComponentModel.DataAnnotations;

namespace Police.Business.ResistanceResponse.Approvals {

    public enum ApprovalStatus {
        
        Created = 0,
        Submitted = 1,
        [Display(Name = "Approved by Supervisor")] ApprovedBySupervisor = 2,
        [Display(Name = "Approved by Training")] ApprovedByTraining = 3,
        [Display(Name = "Rejected by Supervisor")] RejectedBySupervisor = 4,
        [Display(Name = "Rejected by Training")] RejectedByTraining = 5,
        Converted = 6
    }



}
