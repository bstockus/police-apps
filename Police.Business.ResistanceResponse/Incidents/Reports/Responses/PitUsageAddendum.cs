using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public class PitUsageAddendum {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid ResponseId { get; set; }

        public virtual Response Response { get; set; }

        public int PitUsageVehicleSpeed { get; set; }
        public YesNo WasSuspectVehicleImmobilized { get; set; }
        public YesNo WasSecondaryImpactBySuspectVehicleAfterPit { get; set; }
        public string SecondaryImpactBySuspectVehicleAfterPitPartsImpacted { get; set; }

        [AttributeUsage(AttributeTargets.Field)]
        public class PitUsageAddendumRequiredAttribute : Attribute {

            public PitUsageAddendumRequiredAttribute() { }

        }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, PitUsageAddendum> {

            public override void Build(EntityTypeBuilder<PitUsageAddendum> builder) {

                builder.ToTable("PitUsageAddendums", "RestRep");

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
                    rules.RuleFor(_ => _.PitUsageVehicleSpeed).AsPitUsageVehicleSpeed();
                    rules.RuleFor(_ => _.SecondaryImpactBySuspectVehicleAfterPitPartsImpacted).AsYesNoDescription();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Response).WithOne(_ => _.PitUsageAddendum)
                    .HasForeignKey<PitUsageAddendum>(_ => new {
                        _.IncidentId,
                        _.OfficerId,
                        _.SubjectId,
                        _.ResponseId
                    }).OnDelete(DeleteBehavior.Cascade);

            }

        }

    }

}