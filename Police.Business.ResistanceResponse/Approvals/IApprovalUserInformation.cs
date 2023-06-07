namespace Police.Business.ResistanceResponse.Approvals {

    public interface IApprovalUserInformation : IApprovalInformation {

        string SubmitterUserName { get; }
        string SubmitterEmailAddress { get; }

        string SupervisorApproverUserName { get; }
        string SupervisorApproverEmailAddress { get; }

        string TrainingApproverUserName { get; }
        string TrainingApproverEmailAddress { get; }
        
    }

    public interface IApprovalUserCommentsInformation : IApprovalUserInformation {

        string SupervisorsComments { get; }
        string TrainingsComments { get; }

    }

}