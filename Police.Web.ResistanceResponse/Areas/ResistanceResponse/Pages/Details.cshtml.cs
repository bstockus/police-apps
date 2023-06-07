using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class DetailsModel : ResistanceResponsePageModel {

        [BindProperty]
        public Guid IncidentId { get; set; }

        [TempData]
        public string Message { get; set; }

        public IncidentDetailedInfo Incident { get; set; }
        public UserInformation UserInformation { get; set; }
        public OfficerInfo Officer { get; set; }

        public bool CanUserOriginalSubmitIncidentOrAnyChildrenAsOfficer { get; set; }
        public bool CanUserReSubmitIncidentOrAnyChildrenAsOfficer { get; set; }
        public bool CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisorOrTraining { get; set; }
        public bool CanUserDeleteIncident { get; set; }
        public bool CanUserUpdateIncident { get; set; }

        public DetailsModel(IMediator mediator, IUserService userService) :
            base(mediator, userService) { }

        public async Task<ActionResult> OnGet(Guid incidentId) {
            IncidentId = incidentId;
            Incident = await Mediator.Send(new FetchDetailedIncidentQuery(incidentId));
            UserInformation = await FetchCurrentUser();
            Officer = await Mediator.Send(new FetchOfficerByUserIdQuery(UserInformation.UserId));

            if (Incident == null) {
                return NotFound();
            }

            CanUserOriginalSubmitIncidentOrAnyChildrenAsOfficer =
                Incident.CanUserOriginalSubmitIncidentOrAnyChildrenAsOfficer(UserInformation);
            CanUserReSubmitIncidentOrAnyChildrenAsOfficer =
                Incident.CanUserReSubmitIncidentOrAnyChildrenAsOfficer(UserInformation);
            CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisorOrTraining =
                Incident.CanUserApproveOrRejectIncidentOrAnyChildrenAsSupervisorOrTraining(UserInformation);
            CanUserDeleteIncident = Incident.IsUserAllowedToDeleteIncident(UserInformation);
            CanUserUpdateIncident = Incident.IsUserAllowedToUpdateIncident(UserInformation);

            return Page();
        }

        public async Task<ActionResult> OnPostSubmitChangesAsOfficerAsync() {

            await Mediator.Send(new SubmitIncidentForApprovalCommand {
                IncidentId = IncidentId,
                SubmitterId = await FetchCurrentUserId()
            });

            Message = "Your changes have been submitted for approval.";

            return RedirectToDetailsPage(IncidentId);
        }

        public async Task<ActionResult> OnPostSubmitChangesAsApproverAsync() {

            var incidentDetails = await Mediator.Send(new FetchDetailedIncidentQuery(IncidentId));

            var approveOrRejectIncidentCommand = new ApproveOrRejectIncidentCommand {
                IncidentId = IncidentId,
                ApproverId = await FetchCurrentUserId()
            };

            //1. Incident Details
            var incidentData = ParseApprovalOrRejectionData(HttpContext.Request.Form, "Incident");
            if (incidentData.HasValue) {
                approveOrRejectIncidentCommand.IncidentData =
                    new ApproveOrRejectIncidentCommand.IncidentApprovalOrRejectionData {
                        ApprovalOrRejection = incidentData.Value.Choice,
                        Comments = incidentData.Value.Remarks
                    };
            }

            //2. Incident Officer Details
            var incidentOfficerDatas =
                new List<ApproveOrRejectIncidentCommand.IncidentOfficerApprovalOrRejectionData>();
            foreach (var incidentOfficer in incidentDetails.IncidentOfficers) {
                var incidentOfficerData = ParseApprovalOrRejectionData(HttpContext.Request.Form,
                    $"IncidentOfficer_{incidentOfficer.OfficerId.ToString()}");

                if (incidentOfficerData.HasValue) {
                    incidentOfficerDatas.Add(new ApproveOrRejectIncidentCommand.IncidentOfficerApprovalOrRejectionData {
                        ApprovalOrRejection = incidentOfficerData.Value.Choice,
                        Comments = incidentOfficerData.Value.Remarks,
                        OfficerId = incidentOfficer.OfficerId
                    });
                }
            }

            approveOrRejectIncidentCommand.IncidentOfficerDatas = incidentOfficerDatas;

            //3. Subject Details
            var subjectDatas = new List<ApproveOrRejectIncidentCommand.SubjectApprovalOrRejectionData>();
            foreach (var subject in incidentDetails.Subjects) {
                var subjectData = ParseApprovalOrRejectionData(HttpContext.Request.Form,
                    $"Subject_{subject.SubjectId.ToString()}");

                if (subjectData.HasValue) {
                    subjectDatas.Add(new ApproveOrRejectIncidentCommand.SubjectApprovalOrRejectionData {
                        ApprovalOrRejection = subjectData.Value.Choice,
                        Comments = subjectData.Value.Remarks,
                        SubjectId = subject.SubjectId
                    });
                }

            }

            approveOrRejectIncidentCommand.SubjectDatas = subjectDatas;

            //4. Report Details
            var reportDatas = new List<ApproveOrRejectIncidentCommand.ReportApprovalOrRejectionData>();
            foreach (var report in incidentDetails.Reports) {
                var reportData = ParseApprovalOrRejectionData(HttpContext.Request.Form,
                    $"Report_{report.SubjectId.ToString()}_{report.OfficerId.ToString()}");

                if (reportData.HasValue) {
                    reportDatas.Add(new ApproveOrRejectIncidentCommand.ReportApprovalOrRejectionData {
                        ApprovalOrRejection = reportData.Value.Choice,
                        Comments = reportData.Value.Remarks,
                        OfficerId = report.OfficerId,
                        SubjectId = report.SubjectId
                    });
                }
            }

            approveOrRejectIncidentCommand.ReportDatas = reportDatas;

            await Mediator.Send(approveOrRejectIncidentCommand);

            Message = "Your approvals and rejections have been submitted.";

            return RedirectToDetailsPage(IncidentId);

        }

        public struct FormApprovalOrRejectionData {

            public ApprovalOrRejection Choice { get; set; }
            public string Remarks { get; set; }

        }

        private static FormApprovalOrRejectionData? ParseApprovalOrRejectionData(IFormCollection form, string name) {
            
            var choiceKey = $"ApproveOrReject_{name}_Choice";
            var remarksKey = $"ApproveOrReject_{name}_Remarks";

            if (form.ContainsKey(choiceKey) && 
                int.TryParse(form[choiceKey].First(), out var choiceIntValue) && 
                choiceIntValue != -1) {

                var approvalOrRejectionData = new FormApprovalOrRejectionData {
                    Choice = (ApprovalOrRejection) choiceIntValue,
                    Remarks = ""
                };

                if (form.ContainsKey(remarksKey)) {
                    approvalOrRejectionData.Remarks =
                        form[remarksKey].FirstOrDefault(_ => !string.IsNullOrWhiteSpace(_)) ?? "";
                }

                return approvalOrRejectionData;

            }

            return null;

        }

    }

}