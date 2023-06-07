using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lax.Mvc.AdminLte.Bootstrap.Extensions;
using Lax.Serialization.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Police.Business.Common;
using Police.Business.ResistanceResponse.Incidents;
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports;
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Security.Authorization;
using Police.Security.User;
using Police.Web.ResistanceResponse.Infrastructure;

namespace Police.Web.ResistanceResponse.Areas.ResistanceResponse.Pages {

    [Authorize(AuthorizationPolicies.MustBeActiveUser)]
    public class AttachReportModel : ResistanceResponsePageModel {

        private readonly IJsonSerializier _jsonSerializier;

        public class ResponseTypeInfo {

            public string Text { get; set; }
            public string Value { get; set; }
            public bool IsFireArmDeadlyForceAddendumRequired { get; set; }
            public bool IsOtherDeadlyForceAddendumRequired { get; set; }
            public bool IsPitUsageAddendumRequired { get; set; }
            public bool IsTaserUsageAddendumRequired { get; set; }

            public ResponseTypeInfo(ResponseType responseType) {
                Text = responseType.GetDescription();
                Value = ((int) responseType).ToString();
                IsFireArmDeadlyForceAddendumRequired = responseType.IsFireArmDeadlyForceAddendumRequired();
                IsOtherDeadlyForceAddendumRequired = responseType.IsOtherDeadlyForceAddendumRequired();
                IsPitUsageAddendumRequired = responseType.IsPitUsageAddendumRequired();
                IsTaserUsageAddendumRequired = responseType.IsTaserAddendumRequired();
            }

        }

        public class ReportData {

            public class ResistanceEncountered {

                [JsonProperty(PropertyName = "resistanceType")]
                public ResistanceType Resistance { get; set; }

                [JsonProperty(PropertyName = "description")]
                public string Description { get; set; }

                public AttachReportCommand.ResistanceData AsAttachResistanceData() =>
                    new AttachReportCommand.ResistanceData {
                        Resistance = Resistance,
                        Description = Description
                    };

                public UpdateReportCommand.ResistanceData AsUpdateResistanceData() =>
                    new UpdateReportCommand.ResistanceData {
                        Resistance = Resistance,
                        Description = Description
                    };

                public ResistanceEncountered() { }

                public ResistanceEncountered(ReportInfo.ReportResistanceInfo reportResistanceInfo) {
                    Resistance = reportResistanceInfo.ResistanceType;
                    Description = reportResistanceInfo.Description;
                }

            }

            public class ResponseUsed {

                public class FireArmDeadlyForceUsed {

                    [JsonProperty(PropertyName = "fireArmMake")]
                    public string FireArmMake { get; set; }

                    [JsonProperty(PropertyName = "fireArmModel")]
                    public string FireArmModel { get; set; }

                    [JsonProperty(PropertyName = "fireArmSerialNumber")]
                    public string FireArmSerialNumber { get; set; }

                    [JsonProperty(PropertyName = "fireArmAmmoType")]
                    public string FireArmAmmoType { get; set; }

                    public AttachReportCommand.ResponseData.FireArmDeadlyForceAddendumData
                        AsAttachFireArmDeadlyForceAddendumData() =>
                        new AttachReportCommand.ResponseData.FireArmDeadlyForceAddendumData {
                            FireArmMake = FireArmMake,
                            FireArmModel = FireArmModel,
                            FireArmAmmoType = FireArmAmmoType,
                            FireArmSerialNumber = FireArmSerialNumber
                        };

                    public UpdateReportCommand.ResponseData.FireArmDeadlyForceAddendumData
                        AsUpdateFireArmDeadlyForceAddendumData() =>
                        new UpdateReportCommand.ResponseData.FireArmDeadlyForceAddendumData {
                            FireArmMake = FireArmMake,
                            FireArmModel = FireArmModel,
                            FireArmAmmoType = FireArmAmmoType,
                            FireArmSerialNumber = FireArmSerialNumber
                        };

                    public FireArmDeadlyForceUsed() { }

                    public FireArmDeadlyForceUsed(
                        ReportInfo.ReportResponseInfo.FireArmDeadlyForceAddendumInfo fireArmDeadlyForceAddendum) {
                        FireArmMake = fireArmDeadlyForceAddendum.FireArmMake;
                        FireArmModel = fireArmDeadlyForceAddendum.FireArmModel;
                        FireArmSerialNumber = fireArmDeadlyForceAddendum.FireArmSerialNumber;
                        FireArmAmmoType = fireArmDeadlyForceAddendum.FireArmAmmoType;
                    }

                }

                public class OtherDeadlyForceUsed {

                    [JsonProperty(PropertyName = "otherDeadlyForceDescription")]
                    public string OtherDeadlyForceDescription { get; set; }

                    public AttachReportCommand.ResponseData.OtherDeadlyForceAddendumData
                        AsAttachOtherDeadlyForceAddendumData() =>
                        new AttachReportCommand.ResponseData.OtherDeadlyForceAddendumData {
                            OtherDeadlyForceDescription = OtherDeadlyForceDescription
                        };

                    public UpdateReportCommand.ResponseData.OtherDeadlyForceAddendumData
                        AsUpdateOtherDeadlyForceAddendumData() =>
                        new UpdateReportCommand.ResponseData.OtherDeadlyForceAddendumData {
                            OtherDeadlyForceDescription = OtherDeadlyForceDescription
                        };

                    public OtherDeadlyForceUsed() { }

                    public OtherDeadlyForceUsed(
                        ReportInfo.ReportResponseInfo.OtherDeadlyForceAddendumInfo otherDeadlyForceAddendum) {
                        OtherDeadlyForceDescription = otherDeadlyForceAddendum.OtherDeadlyForceDescription;
                    }

                }

                public class PitUsageUsed {

                    [JsonProperty(PropertyName = "pitUsageVehicleSpeed")]
                    public int PitUsageVehicleSpeed { get; set; }

                    [JsonProperty(PropertyName = "wasSuspectVehicleImmobilized")]
                    public YesNo WasSuspectVehicleImmobilized { get; set; }

                    [JsonProperty(PropertyName = "wasSecondaryImpactBySuspectVehicleAfterPit")]
                    public YesNo WasSecondaryImpactBySuspectVehicleAfterPit { get; set; }

                    [JsonProperty(PropertyName = "secondaryImpactBySuspectVehicleAfterPitPartsImpacted")]
                    public string SecondaryImpactBySuspectVehicleAfterPitPartsImpacted { get; set; }

                    public AttachReportCommand.ResponseData.PitUsageAddendumData AsAttachPitUsageAddendumData() =>
                        new AttachReportCommand.ResponseData.PitUsageAddendumData {
                            PitUsageVehicleSpeed = PitUsageVehicleSpeed,
                            WasSuspectVehicleImmobilized = WasSuspectVehicleImmobilized,
                            WasSecondaryImpactBySuspectVehicleAfterPit = WasSecondaryImpactBySuspectVehicleAfterPit,
                            SecondaryImpactBySuspectVehicleAfterPitPartsImpacted =
                                SecondaryImpactBySuspectVehicleAfterPitPartsImpacted
                        };

                    public UpdateReportCommand.ResponseData.PitUsageAddendumData AsUpdatePitUsageAddendumData() =>
                        new UpdateReportCommand.ResponseData.PitUsageAddendumData {
                            PitUsageVehicleSpeed = PitUsageVehicleSpeed,
                            WasSuspectVehicleImmobilized = WasSuspectVehicleImmobilized,
                            WasSecondaryImpactBySuspectVehicleAfterPit = WasSecondaryImpactBySuspectVehicleAfterPit,
                            SecondaryImpactBySuspectVehicleAfterPitPartsImpacted =
                                SecondaryImpactBySuspectVehicleAfterPitPartsImpacted
                        };

                    public PitUsageUsed() { }

                    public PitUsageUsed(ReportInfo.ReportResponseInfo.PitUsageAddendumInfo pitUsageAddendum) {
                        PitUsageVehicleSpeed = pitUsageAddendum.PitUsageVehicleSpeed;
                        WasSuspectVehicleImmobilized = pitUsageAddendum.WasSuspectVehicleImmobilized;
                        WasSecondaryImpactBySuspectVehicleAfterPit =
                            pitUsageAddendum.WasSecondaryImpactBySuspectVehicleAfterPit;
                        SecondaryImpactBySuspectVehicleAfterPitPartsImpacted =
                            pitUsageAddendum.SecondaryImpactBySuspectVehicleAfterPitPartsImpacted;
                    }

                }

                public class TaserUsageUsed {

                    [JsonProperty(PropertyName = "wasLaserDisplayUsed")]
                    public bool WasLaserDisplayUsed { get; set; }

                    [JsonProperty(PropertyName = "wasArcDisplayUsed")]
                    public bool WasArcDisplayUsed { get; set; }

                    [JsonProperty(PropertyName = "wasDriveStunUsed")]
                    public bool WasDriveStunUsed { get; set; }

                    [JsonProperty(PropertyName = "wasProbeDeployUsed")]
                    public bool WasProbeDeployUsed { get; set; }

                    [JsonProperty(PropertyName = "taserSerialNumber")]
                    public string TaserSerialNumber { get; set; }

                    [JsonProperty(PropertyName = "didProbesContact")]
                    public YesNo DidProbesContact { get; set; }

                    [JsonProperty(PropertyName = "cyclesApplied")]
                    public int CyclesApplied { get; set; }

                    [JsonProperty(PropertyName = "distanceWhenLaunched")]
                    public decimal DistanceWhenLaunched { get; set; }

                    [JsonProperty(PropertyName = "distanceBetweenProbes")]
                    public decimal DistanceBetweenProbes { get; set; }

                    [JsonProperty(PropertyName = "additionalShotsRequired")]
                    public YesNo AdditionalShotsRequired { get; set; }

                    [JsonProperty(PropertyName = "subjectWearingHeavyClothing")]
                    public YesNo SubjectWearingHeavyClothing { get; set; }

                    [JsonProperty(PropertyName = "didProbesPenetrateSkin")]
                    public YesNo DidProbesPenetrateSkin { get; set; }

                    [JsonProperty(PropertyName = "wereProbesRemovedAtScene")]
                    public YesNo WereProbesRemovedAtScene { get; set; }

                    [JsonProperty(PropertyName = "whoRemovedProbes")]
                    public string WhoRemovedProbes { get; set; }

                    [JsonProperty(PropertyName = "anySecondaryInjuriesFromTaserUsage")]
                    public YesNo AnySecondaryInjuriesFromTaserUsage { get; set; }

                    [JsonProperty(PropertyName = "wasMedicalAttentionRequiredForSecondaryInjuries")]
                    public YesNo WasMedicalAttentionRequiredForSecondaryInjuries { get; set; }

                    [JsonProperty(PropertyName = "numberOfPhotosTaken")]
                    public int NumberOfPhotosTaken { get; set; }

                    [JsonProperty(PropertyName = "cameraUsedToTakePhotos")]
                    public string CameraUsedToTakePhotos { get; set; }

                    [JsonProperty(PropertyName = "taserCartridgeNumberUsed")]
                    public string TaserCartridgeNumberUsed { get; set; }

                    [JsonProperty(PropertyName = "bodyUsageLocations")]
                    public IEnumerable<BodyUsageLocationUsed> BodyUsageLocations { get; set; } =
                        new List<BodyUsageLocationUsed>();

                    public TaserUsageUsed() { }

                    public TaserUsageUsed(
                        ReportInfo.ReportResponseInfo.TaserUsageAddendumInfo taserUsageAddendum) {

                        WasLaserDisplayUsed = taserUsageAddendum.WasLaserDisplayUsed;
                        WasArcDisplayUsed = taserUsageAddendum.WasArcDisplayUsed;
                        WasDriveStunUsed = taserUsageAddendum.WasDriveStunUsed;
                        WasProbeDeployUsed = taserUsageAddendum.WasProbeDeployUsed;

                        TaserSerialNumber = taserUsageAddendum.TaserSerialNumber;

                        DidProbesContact = taserUsageAddendum.DidProbesContact;
                        CyclesApplied = taserUsageAddendum.CyclesApplied;
                        DistanceWhenLaunched = taserUsageAddendum.DistanceWhenLaunched;
                        DistanceBetweenProbes = taserUsageAddendum.DistanceBetweenProbes;

                        AdditionalShotsRequired = taserUsageAddendum.AdditionalShotsRequired;
                        SubjectWearingHeavyClothing = taserUsageAddendum.SubjectWearingHeavyClothing;
                        DidProbesPenetrateSkin = taserUsageAddendum.DidProbesPenetrateSkin;
                        WereProbesRemovedAtScene = taserUsageAddendum.WereProbesRemovedAtScene;
                        WhoRemovedProbes = taserUsageAddendum.WhoRemovedProbes;
                        AnySecondaryInjuriesFromTaserUsage = taserUsageAddendum.AnySecondaryInjuriesFromTaserUsage;
                        WasMedicalAttentionRequiredForSecondaryInjuries =
                            taserUsageAddendum.WasMedicalAttentionRequiredForSecondaryInjuries;

                        NumberOfPhotosTaken = taserUsageAddendum.NumberOfPhotosTaken;
                        CameraUsedToTakePhotos = taserUsageAddendum.CameraUsedToTakePhotos;
                        TaserCartridgeNumberUsed = taserUsageAddendum.TaserCartridgeNumberUsed;

                        BodyUsageLocations =
                            taserUsageAddendum.BodyUsageLocations.Select(_ => new BodyUsageLocationUsed(_));

                    }

                    public AttachReportCommand.ResponseData.TaserUsageAddendumData
                        AsAttachTaserUsageAddendumData() =>
                        new AttachReportCommand.ResponseData.TaserUsageAddendumData {
                            WasLaserDisplayUsed = WasLaserDisplayUsed,
                            WasArcDisplayUsed = WasArcDisplayUsed,
                            WasDriveStunUsed = WasDriveStunUsed,
                            WasProbeDeployUsed = WasProbeDeployUsed,
                            TaserSerialNumber = TaserSerialNumber,
                            DidProbesContact = DidProbesContact,
                            CyclesApplied = CyclesApplied,
                            DistanceWhenLaunched = DistanceWhenLaunched,
                            DistanceBetweenProbes = DistanceBetweenProbes,
                            AdditionalShotsRequired = AdditionalShotsRequired,
                            SubjectWearingHeavyClothing = SubjectWearingHeavyClothing,
                            DidProbesPenetrateSkin = DidProbesPenetrateSkin,
                            WereProbesRemovedAtScene = WereProbesRemovedAtScene,
                            WhoRemovedProbes = WhoRemovedProbes,
                            AnySecondaryInjuriesFromTaserUsage = AnySecondaryInjuriesFromTaserUsage,
                            WasMedicalAttentionRequiredForSecondaryInjuries =
                                WasMedicalAttentionRequiredForSecondaryInjuries,
                            NumberOfPhotosTaken = NumberOfPhotosTaken,
                            CameraUsedToTakePhotos = CameraUsedToTakePhotos,
                            TaserCartridgeNumberUsed = TaserCartridgeNumberUsed,
                            BodyUsageLocations = BodyUsageLocations.Select(_ => _.AsAttachBodyUsageLocationData())
                                .ToList()
                        };

                    public UpdateReportCommand.ResponseData.TaserUsageAddendumData
                        AsUpdateTaserUsageAddendumData() =>
                        new UpdateReportCommand.ResponseData.TaserUsageAddendumData {
                            WasLaserDisplayUsed = WasLaserDisplayUsed,
                            WasArcDisplayUsed = WasArcDisplayUsed,
                            WasDriveStunUsed = WasDriveStunUsed,
                            WasProbeDeployUsed = WasProbeDeployUsed,
                            TaserSerialNumber = TaserSerialNumber,
                            DidProbesContact = DidProbesContact,
                            CyclesApplied = CyclesApplied,
                            DistanceWhenLaunched = DistanceWhenLaunched,
                            DistanceBetweenProbes = DistanceBetweenProbes,
                            AdditionalShotsRequired = AdditionalShotsRequired,
                            SubjectWearingHeavyClothing = SubjectWearingHeavyClothing,
                            DidProbesPenetrateSkin = DidProbesPenetrateSkin,
                            WereProbesRemovedAtScene = WereProbesRemovedAtScene,
                            WhoRemovedProbes = WhoRemovedProbes,
                            AnySecondaryInjuriesFromTaserUsage = AnySecondaryInjuriesFromTaserUsage,
                            WasMedicalAttentionRequiredForSecondaryInjuries =
                                WasMedicalAttentionRequiredForSecondaryInjuries,
                            NumberOfPhotosTaken = NumberOfPhotosTaken,
                            CameraUsedToTakePhotos = CameraUsedToTakePhotos,
                            TaserCartridgeNumberUsed = TaserCartridgeNumberUsed,
                            BodyUsageLocations = BodyUsageLocations.Select(_ => _.AsUpdateBodyUsageLocationData())
                                .ToList()
                        };

                    public class BodyUsageLocationUsed {

                        [JsonProperty(PropertyName = "bodyUsageType")]
                        public TaserUsageAddendum.BodyUsageLocation.UsageType BodyUsageType { get; set; }

                        [JsonProperty(PropertyName = "x")]
                        public int X { get; set; }

                        [JsonProperty(PropertyName = "y")]
                        public int Y { get; set; }

                        public BodyUsageLocationUsed() { }

                        public BodyUsageLocationUsed(
                            ReportInfo.ReportResponseInfo.TaserUsageAddendumInfo.BodyUsageLocationInfo
                                bodyUsageLocation) {

                            BodyUsageType = bodyUsageLocation.BodyUsageType;
                            X = bodyUsageLocation.X;
                            Y = bodyUsageLocation.Y;

                        }

                        public AttachReportCommand.ResponseData.TaserUsageAddendumData.BodyUsageLocationData
                            AsAttachBodyUsageLocationData() =>
                            new AttachReportCommand.ResponseData.TaserUsageAddendumData.BodyUsageLocationData {
                                BodyUsageType = BodyUsageType,
                                X = X,
                                Y = Y
                            };

                        public UpdateReportCommand.ResponseData.TaserUsageAddendumData.BodyUsageLocationData
                            AsUpdateBodyUsageLocationData() =>
                            new UpdateReportCommand.ResponseData.TaserUsageAddendumData.BodyUsageLocationData {
                                BodyUsageType = BodyUsageType,
                                X = X,
                                Y = Y
                            };

                    }

                }

                [JsonProperty(PropertyName = "responseType")]
                public ResponseType Response { get; set; }

                [JsonProperty(PropertyName = "wasEffective")]
                public YesNo WasEffective { get; set; }

                [JsonProperty(PropertyName = "fireArmDeadlyForce")]
                public FireArmDeadlyForceUsed FireArmDeadlyForce { get; set; }

                [JsonProperty(PropertyName = "otherDeadlyForce")]
                public OtherDeadlyForceUsed OtherDeadlyForce { get; set; }

                [JsonProperty(PropertyName = "pitUsage")]
                public PitUsageUsed PitUsage { get; set; }

                [JsonProperty(PropertyName = "taserUsage")]
                public TaserUsageUsed TaserUsage { get; set; }

                [JsonProperty(PropertyName = "responseId")]
                public string ResponseId { get; set; }

                public AttachReportCommand.ResponseData AsAttachResponseData() =>
                    new AttachReportCommand.ResponseData {
                        Response = Response,
                        WasEffective = WasEffective,
                        FireArmDeadlyForceAddendum = FireArmDeadlyForce?.AsAttachFireArmDeadlyForceAddendumData(),
                        OtherDeadlyForceAddendum = OtherDeadlyForce?.AsAttachOtherDeadlyForceAddendumData(),
                        PitUsageAddendum = PitUsage?.AsAttachPitUsageAddendumData(),
                        TaserUsageAddendum = TaserUsage?.AsAttachTaserUsageAddendumData()
                    };

                public UpdateReportCommand.ResponseData AsUpdateResponseData() =>
                    new UpdateReportCommand.ResponseData {
                        Response = Response,
                        WasEffective = WasEffective,
                        FireArmDeadlyForceAddendum = FireArmDeadlyForce?.AsUpdateFireArmDeadlyForceAddendumData(),
                        OtherDeadlyForceAddendum = OtherDeadlyForce?.AsUpdateOtherDeadlyForceAddendumData(),
                        PitUsageAddendum = PitUsage?.AsUpdatePitUsageAddendumData(),
                        TaserUsageAddendum = TaserUsage?.AsUpdateTaserUsageAddendumData(),
                        ResponseId = ResponseId != null ? Guid.Parse(ResponseId) : (Guid?) null
                    };

                public ResponseUsed() { }

                public ResponseUsed(ReportInfo.ReportResponseInfo reportResponseInfo) {
                    Response = reportResponseInfo.ResponseType;
                    WasEffective = reportResponseInfo.WasEffective;
                    FireArmDeadlyForce = reportResponseInfo.FireArmDeadlyForceAddendum == null
                        ? null
                        : new FireArmDeadlyForceUsed(reportResponseInfo.FireArmDeadlyForceAddendum);
                    OtherDeadlyForce = reportResponseInfo.OtherDeadlyForceAddendum == null
                        ? null
                        : new OtherDeadlyForceUsed(reportResponseInfo.OtherDeadlyForceAddendum);
                    PitUsage = reportResponseInfo.PitUsageAddendum == null
                        ? null
                        : new PitUsageUsed(reportResponseInfo.PitUsageAddendum);
                    TaserUsage = reportResponseInfo.TaserUsageAddendum == null
                        ? null
                        : new TaserUsageUsed(reportResponseInfo.TaserUsageAddendum);
                    ResponseId = reportResponseInfo.Id.ToString();
                }

            }

            [JsonProperty(PropertyName = "resistancesEncountered")]
            public IEnumerable<ResistanceEncountered> Resistances { get; set; }

            [JsonProperty(PropertyName = "responsesUsed")]
            public IEnumerable<ResponseUsed> Responses { get; set; }

        }

        [BindProperty]
        public Guid IncidentId { get; set; }

        [BindProperty]
        public Guid SubjectId { get; set; }

        [BindProperty]
        public Guid OfficerId { get; set; }

        [BindProperty]
        public string Data { get; set; }

        [BindProperty]
        public bool IsNewReport { get; set; }

        public ReportData ExistingReportData { get; set; }

        public ReportInfo ReportInfo { get; set; }
        public IncidentInfo IncidentInfo { get; set; }
        public SubjectInfo SubjectInfo { get; set; }
        public IncidentOfficerInfo OfficerInfo { get; set; }

        public IEnumerable<ResponseTypeInfo> ResponseTypeInfos { get; set; }

        public AttachReportModel(
            IMediator mediator,
            IUserService userService,
            IJsonSerializier jsonSerializier) : base(mediator, userService) {

            _jsonSerializier = jsonSerializier;
        }

        public async Task OnGetAsync(Guid incidentId, Guid officerId, Guid subjectId) {

            IncidentId = incidentId;
            SubjectId = subjectId;
            OfficerId = officerId;

            ReportInfo = await Mediator.Send(new FetchReportQuery(incidentId, subjectId, officerId));
            IsNewReport = ReportInfo == null;

            if (ReportInfo != null) {

                ExistingReportData = new ReportData {
                    Resistances = ReportInfo.Resistances.Select(_ => new ReportData.ResistanceEncountered(_)),
                    Responses = ReportInfo.Responses.Select(_ => new ReportData.ResponseUsed(_))
                };

            }

            IncidentInfo = await Mediator.Send(new FetchIncidentQuery(incidentId));
            SubjectInfo = await Mediator.Send(new FetchSubjectQuery(incidentId, subjectId));
            OfficerInfo = await Mediator.Send(new FetchIncidentOfficerQuery(incidentId, officerId));

            ResponseTypeInfos = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>()
                .Select(_ => new ResponseTypeInfo(_));

        }

        public async Task<IActionResult> OnPostAsync() {

            var userId = (await FetchCurrentUser()).UserId;

            var reportData = _jsonSerializier.Deserialize<ReportData>(Data);

            if (IsNewReport) {
                await Mediator.Send(new AttachReportCommand {
                    IncidentId = IncidentId,
                    OfficerId = OfficerId,
                    SubmitterId = userId,
                    SubjectId = SubjectId,
                    Resistances = reportData.Resistances.Select(_ => _.AsAttachResistanceData()).ToList(),
                    Responses = reportData.Responses.Select(_ => _.AsAttachResponseData()).ToList()
                });
            } else {
                await Mediator.Send(new UpdateReportCommand {
                    IncidentId = IncidentId,
                    OfficerId = OfficerId,
                    SubmitterId = userId,
                    SubjectId = SubjectId,
                    Resistances = reportData.Resistances.Select(_ => _.AsUpdateResistanceData()).ToList(),
                    Responses = reportData.Responses.Select(_ => _.AsUpdateResponseData()).ToList()
                });
            }

            return RedirectToDetailsPage(IncidentId);

        }

    }

}
