using System;
using System.Collections.Generic;
using AutoMapper;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Business.ResistanceResponse.Incidents.Subjects;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    public class ReportInfo : IApprovalUserCommentsInformation {

        public class ReportResistanceInfo {

            public ResistanceType ResistanceType { get; set; }
            public string Description { get; set; }

        }

        public class ReportResponseInfo {

            public class FireArmDeadlyForceAddendumInfo {

                public string FireArmMake { get; set; }
                public string FireArmModel { get; set; }
                public string FireArmSerialNumber { get; set; }
                public string FireArmAmmoType { get; set; }

            }

            public class OtherDeadlyForceAddendumInfo {

                public string OtherDeadlyForceDescription { get; set; }

            }

            public class PitUsageAddendumInfo {

                public int PitUsageVehicleSpeed { get; set; }
                public YesNo WasSuspectVehicleImmobilized { get; set; }
                public YesNo WasSecondaryImpactBySuspectVehicleAfterPit { get; set; }
                public string SecondaryImpactBySuspectVehicleAfterPitPartsImpacted { get; set; }

            }

            public class TaserUsageAddendumInfo {

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
                
                public IEnumerable<BodyUsageLocationInfo> BodyUsageLocations { get; set; }

                public class BodyUsageLocationInfo {

                    public TaserUsageAddendum.BodyUsageLocation.UsageType BodyUsageType { get; set; }
                    public int X { get; set; }
                    public int Y { get; set; }

                }

            }

            public Guid Id { get; set; }
            public ResponseType ResponseType { get; set; }
            public YesNo WasEffective { get; set; }

            public FireArmDeadlyForceAddendumInfo FireArmDeadlyForceAddendum { get; set; }
            public OtherDeadlyForceAddendumInfo OtherDeadlyForceAddendum { get; set; }
            public PitUsageAddendumInfo PitUsageAddendum { get; set; }
            public TaserUsageAddendumInfo TaserUsageAddendum { get; set; }


        }

        public Guid IncidentId { get; set; }

        public Guid OfficerId { get; set; }

        public string OfficerUserName { get; set; }
        public string OfficerFirstName { get; set; }
        public string OfficerLastName { get; set; }
        public string OfficerEmailAddress { get; set; }
        public string OfficerBadgeNumber { get; set; }
        public string OfficerEmployeeNumber { get; set; }
        public string OfficerRank { get; set; }
        public string OfficerJobTitle { get; set; }
        public string OfficerAssignment { get; set; }

        public Guid SubjectId { get; set; }

        public SubjectInfo Subject { get; set; }

        public Guid SubmitterId { get; set; }
        public Guid? SupervisorApproverId { get; set; }
        public Guid? TrainingApproverId { get; set; }
        public string SupervisorsComments { get; set; }
        public string TrainingsComments { get; set; }

        public IEnumerable<ReportResistanceInfo> Resistances { get; set; } = new List<ReportResistanceInfo>();
        public IEnumerable<ReportResponseInfo> Responses { get; set; } = new List<ReportResponseInfo>();

        public ApprovalStatus ApprovalStatus { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Report, ReportInfo>();
                CreateMap<Resistance, ReportResistanceInfo>();
                CreateMap<Response, ReportResponseInfo>();
                CreateMap<FireArmDeadlyForceAddendum, ReportResponseInfo.FireArmDeadlyForceAddendumInfo>();
                CreateMap<OtherDeadlyForceAddendum, ReportResponseInfo.OtherDeadlyForceAddendumInfo>();
                CreateMap<PitUsageAddendum, ReportResponseInfo.PitUsageAddendumInfo>();
                CreateMap<TaserUsageAddendum, ReportResponseInfo.TaserUsageAddendumInfo>();
                CreateMap<TaserUsageAddendum.BodyUsageLocation,
                    ReportResponseInfo.TaserUsageAddendumInfo.BodyUsageLocationInfo>();
            }

        }

        public string SubmitterUserName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterLastName { get; set; }
        public string SubmitterEmailAddress { get; set; }
        public string SupervisorApproverUserName { get; set; }
        public string SupervisorApproverFirstName { get; set; }
        public string SupervisorApproverLastName { get; set; }
        public string SupervisorApproverEmailAddress { get; set; }
        public string TrainingApproverUserName { get; set; }
        public string TrainingApproverFirstName { get; set; }
        public string TrainingApproverLastName { get; set; }
        public string TrainingApproverEmailAddress { get; set; }

    }

}