using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Identity.Users;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents {

    public class Incident : IApprovableEntity {

        public Guid Id { get; set; }

        public DateTime IncidentDateAndTime { get; set; }
        public string IncidentCaseNumber { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public Guid SubmitterId { get; set; }
        public virtual User Submitter { get; set; }

        public Guid? SupervisorApproverId { get; set; }
        public virtual User SupervisorApprover { get; set; }

        public string SupervisorsComments { get; set; }

        public Guid? TrainingApproverId { get; set; }
        public virtual User TrainingApprover { get; set; }

        public string TrainingsComments { get; set; }

        public virtual ICollection<IncidentOfficer> IncidentOfficers { get; set; } = new HashSet<IncidentOfficer>();

        public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();

        public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Incident> {

            public override void Build(EntityTypeBuilder<Incident> builder) {
                builder.ToTable("Incidents", "RestRep");

                builder.HasKey(_ => _.Id);

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.IncidentCaseNumber).AsIncidentCaseNumber();
                    rules.RuleFor(_ => _.SupervisorsComments).AsApproversComments();
                    rules.RuleFor(_ => _.TrainingsComments).AsApproversComments();
                });

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