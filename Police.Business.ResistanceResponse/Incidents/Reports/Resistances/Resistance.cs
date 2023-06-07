using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Resistances {

    public class Resistance {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public virtual Report Report { get; set; }

        public ResistanceType ResistanceType { get; set; }
        public string Description { get; set; }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Resistance> {

            public override void Build(EntityTypeBuilder<Resistance> builder) {
                builder.ToTable("Resistances", "RestRep");

                builder.HasKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId,
                    _.ResistanceType
                });

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.IncidentId).AsEntityIdentity();
                    rules.RuleFor(_ => _.OfficerId).AsEntityIdentity();
                    rules.RuleFor(_ => _.SubjectId).AsEntityIdentity();
                    rules.RuleFor(_ => _.ResistanceType).AsResistanceType();
                    rules.RuleFor(_ => _.Description).AsResistanceEncounteredDescription();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Report).WithMany(_ => _.Resistances).HasForeignKey(_ => new {
                    _.IncidentId,
                    _.OfficerId,
                    _.SubjectId
                }).OnDelete(DeleteBehavior.Cascade);
            }

        }

    }

}