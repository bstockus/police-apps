using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.Organization.Officers;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    public class Report : IApprovableEntity {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public virtual Officer Officer { get; set; }
        public virtual IncidentOfficer IncidentOfficer { get; set; }

        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public Guid SubmitterId { get; set; }
        public virtual User Submitter { get; set; }

        public Guid? SupervisorApproverId { get; set; }
        public virtual User SupervisorApprover { get; set; }

        public string SupervisorsComments { get; set; }

        public Guid? TrainingApproverId { get; set; }
        public virtual User TrainingApprover { get; set; }

        public string TrainingsComments { get; set; }

        public virtual ICollection<Resistance> Resistances { get; set; } = new HashSet<Resistance>();
        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Report> {

            public override void Build(EntityTypeBuilder<Report> builder) {
                builder.ToTable("Reports", "RestRep");

                builder.HasKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId
                });

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.IncidentId).AsEntityIdentity();
                    rules.RuleFor(_ => _.OfficerId).AsEntityIdentity();
                    rules.RuleFor(_ => _.SubjectId).AsEntityIdentity();
                    rules.RuleFor(_ => _.SupervisorsComments).AsApproversComments();
                    rules.RuleFor(_ => _.TrainingsComments).AsApproversComments();
                });

                builder.HasOne(_ => _.Incident).WithMany(_ => _.Reports).HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.IncidentOfficer).WithMany(_ => _.Reports).HasForeignKey(_ => new {
                    _.IncidentId,
                    _.OfficerId
                }).OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Officer).WithMany().HasForeignKey(_ => _.OfficerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Subject).WithMany(_ => _.Reports).HasForeignKey(_ => new {
                    _.IncidentId,
                    _.SubjectId
                }).OnDelete(DeleteBehavior.Restrict);

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