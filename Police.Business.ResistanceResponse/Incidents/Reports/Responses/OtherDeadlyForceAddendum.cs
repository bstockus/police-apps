using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public class OtherDeadlyForceAddendum {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid ResponseId { get; set; }

        public virtual Response Response { get; set; }

        public string OtherDeadlyForceDescription { get; set; }

        [AttributeUsage(AttributeTargets.Field)]
        public class OtherDeadlyForceAddendumRequiredAttribute : Attribute {

            public OtherDeadlyForceAddendumRequiredAttribute() { }

        }

        public class
            EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, OtherDeadlyForceAddendum> {

            public override void Build(EntityTypeBuilder<OtherDeadlyForceAddendum> builder) {

                builder.ToTable("OtherDeadlyForceAddendums", "RestRep");

                builder.HasKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId,
                    _.ResponseId
                });

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.ResponseId).AsEntityIdentity();
                    rules.RuleFor(_ => _.IncidentId).AsEntityIdentity();
                    rules.RuleFor(_ => _.OfficerId).AsEntityIdentity();
                    rules.RuleFor(_ => _.SubjectId).AsEntityIdentity();
                    rules.RuleFor(_ => _.OtherDeadlyForceDescription).AsOtherDeadlyForceDescription();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Response).WithOne(_ => _.OtherDeadlyForceAddendum)
                    .HasForeignKey<OtherDeadlyForceAddendum>(_ => new {
                        _.IncidentId,
                        _.OfficerId,
                        _.SubjectId,
                        _.ResponseId
                    }).OnDelete(DeleteBehavior.Cascade);

            }

        }

    }

}