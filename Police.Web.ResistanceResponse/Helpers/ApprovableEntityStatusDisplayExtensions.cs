using System.Text;
using Police.Business.ResistanceResponse.Approvals;

namespace Police.Web.ResistanceResponse.Helpers {

    public static class ApprovableEntityStatusDisplayExtensions {

        public static string ToApprovalStatusDisplayLabel(this IApprovalInformation approvalInformation) {

            if (approvalInformation.IsUnSubmitted()) {
                return "<span class='label label-info'>Un-submitted</span>";
            }

            if (approvalInformation.IsPending()) {
                return "<span class='label label-warning'>Pending Review</span>";
            }

            if (approvalInformation.IsRejected()) {
                return "<span class='label label-danger'>Rejected</span>";
            }

            if (approvalInformation.IsApproved()) {
                return "<span class='label label-success'>Approved</span>";
            }

            return "<span class='label label-default'>Unknown</span>";

        }

        public static string ToApprovalStatusInformationLine(this IApprovalUserCommentsInformation approvalUserInformation) {

            var results = new StringBuilder("<small class='text-muted'>");

            results.Append($"{(approvalUserInformation.IsUnSubmitted() ? "Created" : "Submitted")} by " +
                           $"<strong>{approvalUserInformation.ToSubmitterNameDisplay()}</strong>. ");

            if (approvalUserInformation.SupervisorApproverId.HasValue &&
                approvalUserInformation.TrainingApproverId.HasValue) {

                results.Append(
                    $"Reviewed by <strong>{approvalUserInformation.ToSupervisorApproverNameDisplay()}</strong> and " +
                    $"<strong>{approvalUserInformation.ToTrainingApproverNameDisplay()}</strong>. ");

            } else if (approvalUserInformation.SupervisorApproverId.HasValue) {

                results.Append(
                    $"Reviewed by <strong>{approvalUserInformation.ToSupervisorApproverNameDisplay()}</strong>. ");

            } else if (approvalUserInformation.TrainingApproverId.HasValue) {

                results.Append(
                    $"Reviewed by <strong>{approvalUserInformation.ToTrainingApproverNameDisplay()}</strong>. ");

            }

            if (!string.IsNullOrWhiteSpace(approvalUserInformation.SupervisorsComments) && approvalUserInformation.SupervisorApproverId.HasValue) {
                results.Append(
                    $"<br/><strong>{approvalUserInformation.ToSupervisorApproverNameDisplay()} Commented: </strong>" +
                    $"<em>{approvalUserInformation.SupervisorsComments}</em>");
            }

            if (!string.IsNullOrWhiteSpace(approvalUserInformation.TrainingsComments) && approvalUserInformation.TrainingApproverId.HasValue) {
                results.Append(
                    $"<br/><strong>{approvalUserInformation.ToTrainingApproverNameDisplay()} Commented: </strong>" +
                    $"<em>{approvalUserInformation.TrainingsComments}</em>");
            }

            results.Append("</small>");

            return results.ToString();

        }

        private static string ToSubmitterNameDisplay(this IApprovalUserInformation approvalUserInformation) =>
            $"{approvalUserInformation.SubmitterUserName}";

        private static string ToSupervisorApproverNameDisplay(this IApprovalUserInformation approvalUserInformation) =>
            approvalUserInformation.SupervisorApproverId.HasValue
                ? $"{approvalUserInformation.SupervisorApproverUserName}"
                : "";

        private static string ToTrainingApproverNameDisplay(this IApprovalUserInformation approvalUserInformation) =>
            approvalUserInformation.TrainingApproverId.HasValue
                ? $"{approvalUserInformation.TrainingApproverUserName}"
                : "";


    }

}