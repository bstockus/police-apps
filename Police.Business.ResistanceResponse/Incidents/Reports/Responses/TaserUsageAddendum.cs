using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public class TaserUsageAddendum {

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public Guid OfficerId { get; set; }
        public Guid SubjectId { get; set; }

        public Guid ResponseId { get; set; }

        public virtual Response Response { get; set; }

        public bool WasLaserDisplayUsed { get; set; }
        public bool WasArcDisplayUsed { get; set; }
        public bool WasDriveStunUsed { get; set; }
        public bool WasProbeDeployUsed { get; set; }

        public string TaserSerialNumber { get; set; }

        public YesNo DidProbesContact { get; set; }
        public int CyclesApplied { get; set; }

        public decimal DistanceWhenLaunched { get; set; }
        public decimal DistanceBetweenProbes { get; set; }

        public YesNo AdditionalShotsRequired { get; set; }
        public YesNo SubjectWearingHeavyClothing { get; set; }
        public YesNo DidProbesPenetrateSkin { get; set; }
        public YesNo WereProbesRemovedAtScene { get; set; }
        public string WhoRemovedProbes { get; set; }
        public YesNo AnySecondaryInjuriesFromTaserUsage { get; set; }
        public YesNo WasMedicalAttentionRequiredForSecondaryInjuries { get; set; }

        public int NumberOfPhotosTaken { get; set; }
        public string CameraUsedToTakePhotos { get; set; }

        public string TaserCartridgeNumberUsed { get; set; }

        public virtual ICollection<BodyUsageLocation> BodyUsageLocations { get; set; } =
            new HashSet<BodyUsageLocation>();

        [AttributeUsage(AttributeTargets.Field)]
        public class TaserUsageAddendumRequiredAttribute : Attribute {

            public TaserUsageAddendumRequiredAttribute() { }

        }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, TaserUsageAddendum> {

            public override void Build(EntityTypeBuilder<TaserUsageAddendum> builder) {

                builder.ToTable("TaserUsageAddendums", "RestRep");

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
                    rules.RuleFor(_ => _.TaserSerialNumber).AsTaserSerialNumber();
                    rules.RuleFor(_ => _.CyclesApplied).AsTaserCyclesApplied();
                    rules.RuleFor(_ => _.DistanceWhenLaunched).AsTaserDistanceWhenLaunched();
                    rules.RuleFor(_ => _.DistanceBetweenProbes).AsTaserDistanceBetweenProbes();
                    rules.RuleFor(_ => _.WhoRemovedProbes).AsTaserWhoRemovedProbes();
                    rules.RuleFor(_ => _.NumberOfPhotosTaken).AsTaserNumberOfPhotosTaken();
                    rules.RuleFor(_ => _.CameraUsedToTakePhotos).AsTaserCameraUsedToTakePhotos();
                    rules.RuleFor(_ => _.TaserCartridgeNumberUsed).AsTaserCartridgeNumberUsed();
                });

                builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(_ => _.Response).WithOne(_ => _.TaserUsageAddendum)
                    .HasForeignKey<TaserUsageAddendum>(_ => new {
                        _.IncidentId,
                        _.OfficerId,
                        _.SubjectId,
                        _.ResponseId
                    }).OnDelete(DeleteBehavior.Cascade);

            }

        }

        public class BodyUsageLocation {

            public enum UsageType {

                ProbeDeploy = 0,
                DriveStun = 1

            }

            public Guid IncidentId { get; set; }
            public virtual Incident Incident { get; set; }

            public Guid OfficerId { get; set; }
            public Guid SubjectId { get; set; }

            public Guid ResponseId { get; set; }
            
            public virtual TaserUsageAddendum TaserUsageAddendum { get; set; }
            
            public UsageType BodyUsageType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, BodyUsageLocation> {

                public override void Build(EntityTypeBuilder<BodyUsageLocation> builder) {
                    
                    builder.ToTable("TaserUsageAddendums.BodyUsageLocations", "RestRep");

                    builder.HasKey(_ => new {
                        _.IncidentId,
                        _.OfficerId,
                        _.SubjectId,
                        _.ResponseId,
                        _.BodyUsageType,
                        _.X,
                        _.Y
                    });
                    
                    builder.FromValidator(rules => {
                        rules.RuleFor(_ => _.ResponseId).AsEntityIdentity();
                        rules.RuleFor(_ => _.IncidentId).AsEntityIdentity();
                        rules.RuleFor(_ => _.OfficerId).AsEntityIdentity();
                        rules.RuleFor(_ => _.SubjectId).AsEntityIdentity();
                        rules.RuleFor(_ => _.X).AsTaserBodyUsageLocationPoint();
                        rules.RuleFor(_ => _.Y).AsTaserBodyUsageLocationPoint();
                    });
                    
                    builder.HasOne(_ => _.Incident).WithMany().HasForeignKey(_ => _.IncidentId)
                        .OnDelete(DeleteBehavior.Restrict);

                    builder.HasOne(_ => _.TaserUsageAddendum).WithMany(_ => _.BodyUsageLocations)
                        .HasForeignKey(_ => new {
                            _.IncidentId,
                            _.OfficerId,
                            _.SubjectId,
                            _.ResponseId
                        }).OnDelete(DeleteBehavior.Cascade);
                    
                }

            }

        }

    }

}