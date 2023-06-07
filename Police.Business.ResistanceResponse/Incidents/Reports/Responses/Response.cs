using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public class Response {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid Id { get; set; }

        public virtual Report Report { get; set; }

        public ResponseType ResponseType { get; set; }
        public YesNo WasEffective { get; set; }

        public virtual FireArmDeadlyForceAddendum FireArmDeadlyForceAddendum { get; set; }
        public virtual OtherDeadlyForceAddendum OtherDeadlyForceAddendum { get; set; }
        public virtual PitUsageAddendum PitUsageAddendum { get; set; }
        public virtual TaserUsageAddendum TaserUsageAddendum { get; set; }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Response> {

            public override void Build(EntityTypeBuilder<Response> builder) {
                builder.ToTable("Responses", "RestRep");

                builder.HasKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId,
                    _.Id
                });

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.Id).AsEntityIdentity();
                    rules.RuleFor(_ => _.IncidentId).AsEntityIdentity();
                    rules.RuleFor(_ => _.OfficerId).AsEntityIdentity();
                    rules.RuleFor(_ => _.SubjectId).AsEntityIdentity();
                    rules.RuleFor(_ => _.ResponseType).AsResponseType();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Report).WithMany(_ => _.Responses).HasForeignKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId
                }).OnDelete(DeleteBehavior.Cascade);
            }

        }

    }

}