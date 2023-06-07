using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Police.Business.ResistanceResponse.Approvals;
using Police.Security.User;

namespace Police.Web.ResistanceResponse.Helpers {

    public static class ApprovableEntitySelectListExtensions {

        public static IEnumerable<SelectListItem> ToSelectListItems(this IApprovalInformation approvalInformation,
            UserInformation user) {

            var results = new List<SelectListItem>();

            results.Add(new SelectListItem("---", "-1", true));

            if (approvalInformation.IsUserAllowedToApproveAsSupervisor(user)) {
                results.Add(new SelectListItem("Approve as Supervisor",
                    ((int) ApprovalOrRejection.SupervisorApprove).ToString()));
            }

            if (approvalInformation.IsUserAllowedToRejectAsSupervisor(user)) {
                results.Add(new SelectListItem("Reject as Supervisor",
                    ((int)ApprovalOrRejection.SupervisorReject).ToString()));
            }

            if (approvalInformation.IsUserAllowedToApproveAsTraining(user)) {
                results.Add(new SelectListItem("Approve as Training",
                    ((int)ApprovalOrRejection.TrainingApprove).ToString()));
            }

            if (approvalInformation.IsUserAllowedToRejectAsTraining(user)) {
                results.Add(new SelectListItem("Reject as Training",
                    ((int)ApprovalOrRejection.TrainingReject).ToString()));
            }

            return results;

        }

    }

}
