using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    public class EmailNotificationsManager : IEmailNotificationsManager {

        private readonly IConfiguration _configuration;


        public EmailNotificationsManager(
            IConfiguration configuration) {
            _configuration = configuration;
        }

        private async Task SendEmailMessage(
            MailAddress to,
            string subject,
            string body,
            string[] ccs = null) {

            var overrideAddress = _configuration.GetSection("Email").GetValue<string>("OverrideAddress", null);
            var mailServer = _configuration.GetSection("Email").GetValue("MailServer", "");

            var fromAddress = _configuration.GetSection("Email").GetValue("FromAddress", "");
            var fromDisplayName = _configuration.GetSection("Email").GetValue("FromDisplayName", "");

            if (!string.IsNullOrWhiteSpace(overrideAddress)) {

                body += $"<br><br>Actual To Address: {to}";

                if (ccs != null) {
                    body += "<br>Actual CC Address:";
                    body = ccs.Aggregate(body, (current, cc) => current + $"{cc}, ");
                }

            }

            var mailMessage = new MailMessage(
                new MailAddress(fromAddress, fromDisplayName),
                string.IsNullOrWhiteSpace(overrideAddress) ? to : new MailAddress(overrideAddress)) {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            };

            if (ccs != null && string.IsNullOrWhiteSpace(overrideAddress)) {
                foreach (var cc in ccs) {
                    mailMessage.CC.Add(new MailAddress(cc));
                }
            }


            var client = new SmtpClient(mailServer);
            client.Send(mailMessage);

        }

        public async Task OfficerSubmitNotification(
            SubmitIncidentForApprovalCommand submitIncidentForApprovalCommand,
            Incident incident,
            UserInformation userInformation) {

            var toAddress = _configuration.GetSection("Notifications").GetSection("OfficerSubmitNotification")
                .GetValue("To", "");

            var ccAddresses = _configuration.GetSection("Notifications").GetSection("OfficerSubmitNotification")
                .GetValue("To", new string[] { });

            await SendEmailMessage(
                new MailAddress(toAddress),
                "A new Response to Resistance Form has been Submitted for Review",
                $"A new Response to Resistance Form has been Submitted for Review.<br/><br/>Case Number: " +
                $"<b>{incident.IncidentCaseNumber}</b>.<br/><br/>" +
                $"<a href='http://police-apps.cityoflacrosse.org/ResistanceResponse/Details?incidentId={incident.Id}'>" +
                $"Click here to review the submission</a>",
                ccAddresses);

        }

        public async Task TaserSubmitNotification(
            SubmitIncidentForApprovalCommand submitIncidentForApprovalCommand,
            Incident incident,
            Report report,
            UserInformation userInformation) {

            var toAddresses = _configuration.GetSection("Notifications")
                .GetValue("TaserSubmitNotification", new string[] { });

            var taserResponse = report.Responses.FirstOrDefault(_ => _.ResponseType.Equals(ResponseType.Taser));
            
            if (taserResponse?.TaserUsageAddendum != null) {
                var responseType =
                    (taserResponse.TaserUsageAddendum.WasArcDisplayUsed ? "[Arc Display] " : "") +
                    (taserResponse.TaserUsageAddendum.WasLaserDisplayUsed ? "[Laser Display] " : "") +
                    (taserResponse.TaserUsageAddendum.WasDriveStunUsed ? "[Drive Stun] " : "") +
                    (taserResponse.TaserUsageAddendum.WasProbeDeployUsed ? "[Probe Deploy] " : "");


                foreach (var emailAddress in toAddresses) {
                    await SendEmailMessage(
                        new MailAddress(emailAddress),
                        "A new Response to Resistance Form has been Submitted indicating a Taser was Used",
                        $"A new Response to Resistance Form has been Submitted for indicating a Taser was Used.<br/><br/>" +
                        $"Case Number: <b>{incident.IncidentCaseNumber}</b>.<br/>" +
                        $"Incident Date &amp; Time: <b>{incident.IncidentDateAndTime.ToShortDateString()} {incident.IncidentDateAndTime.ToShortTimeString()}</b>.<br/>" +
                        $"Officer: <b>{report.Officer.LastName}, {report.Officer.FirstName} ({report.Officer.BadgeNumber})</b>.<br/>" +
                        $"Deployment Types: <b>{responseType}</b><br/>" +
                        $"Taser Serial Number: <b>{taserResponse.TaserUsageAddendum.TaserSerialNumber ?? ""}</b><br/>" +
                        $"Taser Cartridge Number: <b>{taserResponse.TaserUsageAddendum.TaserCartridgeNumberUsed ?? ""}</b><br/>");
                }


            }

        }

        public async Task ApproveOrRejectNotification(
            ApproveOrRejectIncidentCommand approveOrRejectIncidentCommand,
            Incident incident,
            UserInformation userInformation) {

            var supervisorApprovalNotificationToAddresses = _configuration.GetSection("Notifications")
                .GetValue("SupervisorApprovalNotification", new string[] { });

            if ((approveOrRejectIncidentCommand?.IncidentData?.IsRejection() ?? false) ||
                approveOrRejectIncidentCommand.IncidentOfficerDatas.Any(_ => _.IsRejection()) ||
                approveOrRejectIncidentCommand.SubjectDatas.Any(_ => _.IsRejection()) ||
                approveOrRejectIncidentCommand.ReportDatas.Any(_ => _.IsRejection())) {

                var officersToNotify = new List<string>();

                if (approveOrRejectIncidentCommand.IncidentData?.IsRejection() ?? false) {
                    officersToNotify.Add(incident.Submitter.EmailAddress);
                }

                officersToNotify.AddRange(approveOrRejectIncidentCommand.IncidentOfficerDatas
                    .Where(_ => _.IsRejection()).Select(_ =>
                        incident.IncidentOfficers.FirstOrDefault(x => x.OfficerId.Equals(_.OfficerId))?.Submitter
                            ?.EmailAddress ?? ""));

                officersToNotify.AddRange(approveOrRejectIncidentCommand.SubjectDatas
                    .Where(_ => _.IsRejection()).Select(_ =>
                        incident.Subjects.FirstOrDefault(x => x.SubjectId.Equals(_.SubjectId))?.Submitter
                            ?.EmailAddress ?? ""));

                officersToNotify.AddRange(approveOrRejectIncidentCommand.ReportDatas
                    .Where(_ => _.IsRejection()).Select(_ =>
                        incident.Reports
                            .FirstOrDefault(x => x.SubjectId.Equals(_.SubjectId) && x.OfficerId.Equals(_.OfficerId))
                            ?.Submitter?.EmailAddress ?? ""));

                var groupedOfficersToNotify = officersToNotify.GroupBy(_ => _).Select(_ => _.Key);

                foreach (var officerToNotify in groupedOfficersToNotify) {

                    await SendEmailMessage(
                        new MailAddress(officerToNotify),
                        "A Response to Resistance Form that you Submitted has been Rejected",
                        "A Response to Resistance Form that you Submitted has been Rejected.<br/><br/>Case Number: " +
                        $"<b>{incident.IncidentCaseNumber}</b>.<br/><br/>" +
                        $"<a href='http://police-apps.cityoflacrosse.org/ResistanceResponse/Details?incidentId={incident.Id}'>" +
                        $"Click here to view the report you submitted</a>");

                }

            }

            if ((approveOrRejectIncidentCommand?.IncidentData?.IsSupervisorApproval() ?? false) ||
                approveOrRejectIncidentCommand.IncidentOfficerDatas.Any(_ => _.IsSupervisorApproval()) ||
                approveOrRejectIncidentCommand.SubjectDatas.Any(_ => _.IsSupervisorApproval()) ||
                approveOrRejectIncidentCommand.ReportDatas.Any(_ => _.IsSupervisorApproval())) {

                foreach (var emailAddress in supervisorApprovalNotificationToAddresses) {
                    await SendEmailMessage(
                        new MailAddress(emailAddress),
                        "A Response to Resistance Form has been Approved by a Supervisor",
                        "A Response to Resistance Form has been Approved by a Supervisor and is ready for your approval.<br/><br/>Case Number: " +
                        $"<b>{incident.IncidentCaseNumber}</b>.<br/><br/>" +
                        $"<a href='http://police-apps.cityoflacrosse.org/ResistanceResponse/Details?incidentId={incident.Id}'>" +
                        $"Click here to review the submission</a>");
                }

            }

        }

    }

}