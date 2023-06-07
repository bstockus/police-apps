using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.ResistanceResponse.Approvals;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class ApproveOrRejectIncidentCommand : IRequest {

        public abstract class ApprovalOrRejectionDataValidator<TData> : AbstractValidator<TData>
            where TData : ApprovalOrRejectionData {

            protected ApprovalOrRejectionDataValidator() {
                RuleFor(_ => _.Comments).AsApproversComments();
            }

        }

        public class IncidentApprovalOrRejectionData : ApprovalOrRejectionData {

            public class
                IncidentApprovalOrRejectionDataValidator : ApprovalOrRejectionDataValidator<
                    IncidentApprovalOrRejectionData> { }

        }

        public class IncidentOfficerApprovalOrRejectionData : ApprovalOrRejectionData {

            public Guid OfficerId { get; set; }

            public class
                IncidentOfficerApprovalOrRejectionDataValidator : ApprovalOrRejectionDataValidator<
                    IncidentOfficerApprovalOrRejectionData> {

                public IncidentOfficerApprovalOrRejectionDataValidator() {
                    RuleFor(_ => _.OfficerId).AsEntityIdentity();
                }

            }

        }

        public class SubjectApprovalOrRejectionData : ApprovalOrRejectionData {

            public Guid SubjectId { get; set; }

            public class
                SubjectApprovalOrRejectionDataValidator : ApprovalOrRejectionDataValidator<
                    SubjectApprovalOrRejectionData> {

                public SubjectApprovalOrRejectionDataValidator() : base() {
                    RuleFor(_ => _.SubjectId).AsEntityIdentity();
                }

            }

        }

        public class ReportApprovalOrRejectionData : ApprovalOrRejectionData {

            public Guid OfficerId { get; set; }
            public Guid SubjectId { get; set; }

            public class
                ReportApprovalOrRejectionDataValidator : ApprovalOrRejectionDataValidator<
                    ReportApprovalOrRejectionData> {

                public ReportApprovalOrRejectionDataValidator() : base() {
                    RuleFor(_ => _.OfficerId).AsEntityIdentity();
                    RuleFor(_ => _.SubjectId).AsEntityIdentity();
                }

            }

        }

        public Guid IncidentId { get; set; }
        public Guid ApproverId { get; set; }

        public IncidentApprovalOrRejectionData IncidentData { get; set; }

        public IEnumerable<IncidentOfficerApprovalOrRejectionData> IncidentOfficerDatas { get; set; } =
            new List<IncidentOfficerApprovalOrRejectionData>();

        public IEnumerable<SubjectApprovalOrRejectionData> SubjectDatas { get; set; } =
            new List<SubjectApprovalOrRejectionData>();

        public IEnumerable<ReportApprovalOrRejectionData> ReportDatas { get; set; } =
            new List<ReportApprovalOrRejectionData>();

        public class Validator : AbstractValidator<ApproveOrRejectIncidentCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.ApproverId).AsEntityIdentity();
                RuleFor(_ => _.IncidentData)
                    .SetValidator(new IncidentApprovalOrRejectionData.IncidentApprovalOrRejectionDataValidator());
                RuleForEach(_ => _.IncidentOfficerDatas).SetValidator(
                    new IncidentOfficerApprovalOrRejectionData.IncidentOfficerApprovalOrRejectionDataValidator());
                RuleForEach(_ => _.SubjectDatas)
                    .SetValidator(new SubjectApprovalOrRejectionData.SubjectApprovalOrRejectionDataValidator());
                RuleForEach(_ => _.ReportDatas)
                    .SetValidator(new ReportApprovalOrRejectionData.ReportApprovalOrRejectionDataValidator());
            }

        }

        public class Handler : IRequestHandler<ApproveOrRejectIncidentCommand> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<User> _users;
            private readonly IUserService _userService;
            private readonly IEmailNotificationsManager _emailNotificationsManager;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<User> users,
                IUserService userService,
                IEmailNotificationsManager emailNotificationsManager) {

                _incidents = incidents;
                _users = users;
                _userService = userService;
                _emailNotificationsManager = emailNotificationsManager;
            }

            private static void PerformUpdateOnEntity(IApprovableEntity entity,
                ApprovalOrRejectionData approvalOrRejectionData, UserInformation user) {
                if (!entity.AsApprovalInformation().IsUserAllowedToPerformActionOnEntity(user,
                    approvalOrRejectionData.ApprovalOrRejection.ToApprovalEntityAction())) {
                    return;
                }

                entity.GetApprovalStatusStateMachine()
                    .Fire(approvalOrRejectionData.ApprovalOrRejection.ToApprovalEntityAction());

                if (approvalOrRejectionData.ApprovalOrRejection.IsApprovalEntityActionForSupervisor()) {
                    entity.SupervisorApproverId = user.UserId;
                    entity.SupervisorsComments = approvalOrRejectionData.Comments;
                } else {
                    entity.TrainingApproverId = user.UserId;
                    entity.TrainingsComments = approvalOrRejectionData.Comments;
                }

            }

            public async Task<Unit> Handle(ApproveOrRejectIncidentCommand request,
                CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.ApproverId, cancellationToken);

                var incident = await _incidents
                    .Include(_ => _.Submitter)
                    .Include(_ => _.IncidentOfficers).ThenInclude(_ => _.Submitter)
                    .Include(_ => _.Subjects).ThenInclude(_ => _.Submitter)
                    .Include(_ => _.Reports).ThenInclude(_ => _.Submitter)
                    .FirstOrDefaultAsync(_ => _.Id.Equals(request.IncidentId), cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.ApproverId);

                if (request.IncidentData != null) {

                    PerformUpdateOnEntity(
                        incident,
                        request.IncidentData,
                        user);
                }

                foreach (var incidentOfficerData in request.IncidentOfficerDatas) {

                    PerformUpdateOnEntity(
                        incident.IncidentOfficers.FirstOrDefault(_ =>
                            _.OfficerId.Equals(incidentOfficerData.OfficerId)),
                        incidentOfficerData,
                        user);

                }

                foreach (var subjectData in request.SubjectDatas) {

                    PerformUpdateOnEntity(
                        incident.Subjects.FirstOrDefault(_ =>
                            _.SubjectId.Equals(subjectData.SubjectId)),
                        subjectData,
                        user);

                }

                foreach (var reportData in request.ReportDatas) {

                    PerformUpdateOnEntity(
                        incident.Reports.FirstOrDefault(_ =>
                            _.OfficerId.Equals(reportData.OfficerId) && _.SubjectId.Equals(reportData.SubjectId)),
                        reportData,
                        user);

                }

                await _emailNotificationsManager.ApproveOrRejectNotification(
                    request,
                    incident,
                    user);

                return Unit.Value;


            }

        }

    }

}