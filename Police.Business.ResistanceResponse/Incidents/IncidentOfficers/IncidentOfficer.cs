using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.IncidentOfficers {

    public class IncidentOfficer : IApprovableEntity {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public virtual Officer Officer { get; set; }

        public YesNo WasOfficerInjured { get; set; }
        public YesNo DidOfficerRequireMedicalAttention { get; set; }
        public string DidOfficerRequireMedicalAttentionDescription { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public Guid SubmitterId { get; set; }
        public virtual User Submitter { get; set; }

        public Guid? SupervisorApproverId { get; set; }
        public virtual User SupervisorApprover { get; set; }
        
        public string SupervisorsComments { get; set; }

        public Guid? TrainingApproverId { get; set; }
        public virtual User TrainingApprover { get; set; }

        public string TrainingsComments { get; set; }

        public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, IncidentOfficer> {

            public override void Build(EntityTypeBuilder<IncidentOfficer> builder) {
                builder.ToTable("IncidentOfficers", "RestRep");

                builder.HasKey(_ => new {
                    _.IncidentId,
                    _.OfficerId
                });

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.DidOfficerRequireMedicalAttentionDescription).AsYesNoDescription();
                    rules.RuleFor(_ => _.SupervisorsComments).AsApproversComments();
                    rules.RuleFor(_ => _.TrainingsComments).AsApproversComments();
                });

                builder.HasOne(_ => _.Incident).WithMany(_ => _.IncidentOfficers).HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(_ => _.Officer).WithMany().HasForeignKey(_ => _.OfficerId)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(_ => _.Submitter).WithMany().HasForeignKey(_ => _.SubmitterId)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(_ => _.SupervisorApprover).WithMany().HasForeignKey(_ => _.SupervisorApproverId)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(_ => _.TrainingApprover).WithMany().HasForeignKey(_ => _.TrainingApproverId)
                    .OnDelete(DeleteBehavior.Restrict);
            }

        }

    }

}