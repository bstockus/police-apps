using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public class FireArmDeadlyForceAddendum {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid ResponseId { get; set; }

        public virtual Response Response { get; set; }

        public string FireArmMake { get; set; }
        public string FireArmModel { get; set; }
        public string FireArmSerialNumber { get; set; }
        public string FireArmAmmoType { get; set; }
        public string FireArmDescription { get; set; }

        [AttributeUsage(AttributeTargets.Field)]
        public class FireArmDeadlyForceAddendumRequiredAttribute : Attribute {

            public FireArmDeadlyForceAddendumRequiredAttribute() { }
            

        }

        public class
            EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, FireArmDeadlyForceAddendum> {

            public override void Build(EntityTypeBuilder<FireArmDeadlyForceAddendum> builder) {

                builder.ToTable("FireArmDeadlyForceAddendums", "RestRep");

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
                    rules.RuleFor(_ => _.FireArmMake).AsFireArmMake();
                    rules.RuleFor(_ => _.FireArmModel).AsFireArmModel();
                    rules.RuleFor(_ => _.FireArmSerialNumber).AsFireArmSerialNumber();
                    rules.RuleFor(_ => _.FireArmAmmoType).AsFireArmAmmoType();
                    rules.RuleFor(_ => _.FireArmDescription).AsFireArmDescription();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Response).WithOne(_ => _.FireArmDeadlyForceAddendum)
                    .HasForeignKey<FireArmDeadlyForceAddendum>(_ => new {
                        _.IncidentId,
                        _.OfficerId,
                        _.SubjectId,
                        _.ResponseId
                    }).OnDelete(DeleteBehavior.Cascade);

            }

        }

    }

}