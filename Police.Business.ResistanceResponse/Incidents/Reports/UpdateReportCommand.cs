using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Police.Business.Common;
using Police.Business.Identity.Users;
using Police.Business.ResistanceResponse.Approvals;
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class UpdateReportCommand : IRequest {

        public class ResistanceData {

            public ResistanceType Resistance { get; set; }
            public string Description { get; set; }

            public class ResistanceDataValidator : AbstractValidator<ResistanceData> {

                public ResistanceDataValidator() {
                    RuleFor(_ => _.Resistance).AsResistanceType();
                    RuleFor(_ => _.Description).AsResistanceEncounteredDescription();
                }

            }

            public Resistance AsResistance() =>
                new Resistance {
                    ResistanceType = Resistance,
                    Description = Description
                };

        }

        public class ResponseData {

            public class FireArmDeadlyForceAddendumData {

                public string FireArmMake { get; set; }
                public string FireArmModel { get; set; }
                public string FireArmSerialNumber { get; set; }
                public string FireArmAmmoType { get; set; }

                public class FireArmDeadlyForceAddendumDataValidator : AbstractValidator<FireArmDeadlyForceAddendumData> {

                    public FireArmDeadlyForceAddendumDataValidator() {
                        RuleFor(_ => _.FireArmMake).AsFireArmMake();
                        RuleFor(_ => _.FireArmModel).AsFireArmModel();
                        RuleFor(_ => _.FireArmSerialNumber).AsFireArmSerialNumber();
                        RuleFor(_ => _.FireArmAmmoType).AsFireArmAmmoType();
                    }

                }

                public FireArmDeadlyForceAddendum AsFireArmDeadlyForceAddendum() =>
                    new FireArmDeadlyForceAddendum {
                        FireArmMake = FireArmMake,
                        FireArmModel = FireArmModel,
                        FireArmSerialNumber = FireArmSerialNumber,
                        FireArmAmmoType = FireArmAmmoType
                    };

            }

            public class OtherDeadlyForceAddendumData {

                public string OtherDeadlyForceDescription { get; set; }

                public class OtherDeadlyForceAddendumDataValidator : AbstractValidator<OtherDeadlyForceAddendumData> {

                    public OtherDeadlyForceAddendumDataValidator() {
                        RuleFor(_ => _.OtherDeadlyForceDescription).AsOtherDeadlyForceDescription();
                    }

                }

                public OtherDeadlyForceAddendum AsOtherDeadlyForceAddendum() =>
                    new OtherDeadlyForceAddendum {
                        OtherDeadlyForceDescription = OtherDeadlyForceDescription
                    };

            }

            public class PitUsageAddendumData {

                public int PitUsageVehicleSpeed { get; set; }
                public YesNo WasSuspectVehicleImmobilized { get; set; }
                public YesNo WasSecondaryImpactBySuspectVehicleAfterPit { get; set; }
                public string SecondaryImpactBySuspectVehicleAfterPitPartsImpacted { get; set; }

                public class PitUsageAddendumDataValidator : AbstractValidator<PitUsageAddendumData> {

                    public PitUsageAddendumDataValidator() {
                        RuleFor(_ => _.PitUsageVehicleSpeed).AsPitUsageVehicleSpeed();
                        RuleFor(_ => _.SecondaryImpactBySuspectVehicleAfterPitPartsImpacted).AsYesNoDescription();
                    }

                }

                public PitUsageAddendum AsPitUsageAddendum() =>
                    new PitUsageAddendum {
                        PitUsageVehicleSpeed = PitUsageVehicleSpeed,
                        WasSuspectVehicleImmobilized = WasSuspectVehicleImmobilized,
                        WasSecondaryImpactBySuspectVehicleAfterPit = WasSecondaryImpactBySuspectVehicleAfterPit,
                        SecondaryImpactBySuspectVehicleAfterPitPartsImpacted =
                            SecondaryImpactBySuspectVehicleAfterPitPartsImpacted
                    };

            }

            public class TaserUsageAddendumData {

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
                
                public ICollection<BodyUsageLocationData> BodyUsageLocations { get; set; } =
                    new List<BodyUsageLocationData>();

                public class TaserUsageAddendumDataValidator : AbstractValidator<TaserUsageAddendumData> {

                    public TaserUsageAddendumDataValidator() {
                        RuleFor(_ => _.TaserSerialNumber).AsTaserSerialNumber();
                        RuleFor(_ => _.CyclesApplied).AsTaserCyclesApplied();
                        RuleFor(_ => _.DistanceWhenLaunched).AsTaserDistanceWhenLaunched();
                        RuleFor(_ => _.DistanceBetweenProbes).AsTaserDistanceBetweenProbes();
                        RuleFor(_ => _.WhoRemovedProbes).AsTaserWhoRemovedProbes();
                        RuleFor(_ => _.NumberOfPhotosTaken).AsTaserNumberOfPhotosTaken();
                        RuleFor(_ => _.CameraUsedToTakePhotos).AsTaserCameraUsedToTakePhotos();
                        RuleFor(_ => _.TaserCartridgeNumberUsed).AsTaserCartridgeNumberUsed();
                        RuleForEach(_ => _.BodyUsageLocations)
                            .SetValidator(new BodyUsageLocationData.BodyUsageLocationDataValidator());
                    }

                }

                public TaserUsageAddendum AsTaserUsageAddendum() =>
                    new TaserUsageAddendum {
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
                        WasMedicalAttentionRequiredForSecondaryInjuries = WasMedicalAttentionRequiredForSecondaryInjuries,
                        NumberOfPhotosTaken = NumberOfPhotosTaken,
                        CameraUsedToTakePhotos = CameraUsedToTakePhotos,
                        TaserCartridgeNumberUsed = TaserCartridgeNumberUsed,
                        BodyUsageLocations = BodyUsageLocations.Select(_ => _.AsBodyUsageLocation()).ToList()
                    };
                
                public class BodyUsageLocationData {

                    public TaserUsageAddendum.BodyUsageLocation.UsageType BodyUsageType { get; set; }
                    public int X { get; set; }
                    public int Y { get; set; }

                    public class BodyUsageLocationDataValidator : AbstractValidator<BodyUsageLocationData> {

                        public BodyUsageLocationDataValidator() {
                            RuleFor(_ => _.X).AsTaserBodyUsageLocationPoint();
                            RuleFor(_ => _.Y).AsTaserBodyUsageLocationPoint();
                        }

                    }

                    public TaserUsageAddendum.BodyUsageLocation AsBodyUsageLocation() =>
                        new TaserUsageAddendum.BodyUsageLocation {
                            BodyUsageType = BodyUsageType,
                            X = X,
                            Y = Y
                        };

                }

            }

            public ResponseType Response { get; set; }
            public YesNo WasEffective { get; set; }

            public Guid? ResponseId { get; set; }

            public FireArmDeadlyForceAddendumData FireArmDeadlyForceAddendum { get; set; }
            public OtherDeadlyForceAddendumData OtherDeadlyForceAddendum { get; set; }
            public PitUsageAddendumData PitUsageAddendum { get; set; }
            public TaserUsageAddendumData TaserUsageAddendum { get; set; }

            public class ResponseDataValidator : AbstractValidator<ResponseData> {

                public ResponseDataValidator() {
                    RuleFor(_ => _.Response).AsResponseType();
                    RuleFor(_ => _.FireArmDeadlyForceAddendum)
                        .SetValidator(new FireArmDeadlyForceAddendumData.FireArmDeadlyForceAddendumDataValidator());
                    RuleFor(_ => _.OtherDeadlyForceAddendum)
                        .SetValidator(new OtherDeadlyForceAddendumData.OtherDeadlyForceAddendumDataValidator());
                    RuleFor(_ => _.PitUsageAddendum)
                        .SetValidator(new PitUsageAddendumData.PitUsageAddendumDataValidator());
                    RuleFor(_ => _.TaserUsageAddendum)
                        .SetValidator(new TaserUsageAddendumData.TaserUsageAddendumDataValidator());
                }

            }

            public Response AsResponse() => new Response {
                ResponseType = Response,
                WasEffective = WasEffective,
                Id = Guid.NewGuid(),
                FireArmDeadlyForceAddendum = FireArmDeadlyForceAddendum?.AsFireArmDeadlyForceAddendum(),
                OtherDeadlyForceAddendum = OtherDeadlyForceAddendum?.AsOtherDeadlyForceAddendum(),
                PitUsageAddendum = PitUsageAddendum?.AsPitUsageAddendum(),
                TaserUsageAddendum = TaserUsageAddendum?.AsTaserUsageAddendum()
            };

        }

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid OfficerId { get; set; }
        public Guid SubmitterId { get; set; }
        public ICollection<ResistanceData> Resistances { get; set; } = new List<ResistanceData>();
        public ICollection<ResponseData> Responses { get; set; } = new List<ResponseData>();

        public class Validator : AbstractValidator<UpdateReportCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
                RuleFor(_ => _.OfficerId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleForEach(_ => _.Resistances).SetValidator(new ResistanceData.ResistanceDataValidator());
                RuleForEach(_ => _.Responses).SetValidator(new ResponseData.ResponseDataValidator());
            }

        }

        public class Handler : IRequestHandler<UpdateReportCommand> {
            
            private readonly DbSet<Report> _reports;
            private readonly DbSet<User> _users;
            private readonly DbSet<Resistance> _resistances;
            private readonly DbSet<Response> _responses;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Report> reports,
                DbSet<User> users,
                DbSet<Resistance> resistances,
                DbSet<Response> responses,
                IUserService userService) {

                _reports = reports;
                _users = users;
                _resistances = resistances;
                _responses = responses;
                _userService = userService;
            }

            public async Task<Unit> Handle(UpdateReportCommand request, CancellationToken cancellationToken) {
                
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _reports.ThrowIfReportDoesNotExist(request.IncidentId, request.OfficerId, request.SubjectId,
                    cancellationToken);

                foreach (var response in request.Responses) {

                    if (response.Response.IsFireArmDeadlyForceAddendumRequired() &&
                        response.FireArmDeadlyForceAddendum == null) {

                        throw new Exception(
                            $"FireArmDeadlyForceAddendum Required when ResponseType is {response.Response}");
                    }

                    if (response.Response.IsOtherDeadlyForceAddendumRequired() &&
                        response.OtherDeadlyForceAddendum == null) {

                        throw new Exception(
                            $"OtherDeadlyForceAddendum Required when ResponseType is {response.Response}");
                    }

                    if (response.Response.IsPitUsageAddendumRequired() &&
                        response.PitUsageAddendum == null) {

                        throw new Exception(
                            $"PitUsageAddendum Required when ResponseType is {response.Response}");
                    }

                    if (response.Response.IsTaserAddendumRequired() &&
                        response.TaserUsageAddendum == null) {

                        throw new Exception(
                            $"TaserUsageAddendum Required when ResponseType is {response.Response}");

                    }

                }

                var report =
                    await _reports.Include(_ => _.Resistances)
                        .Include(_ => _.Responses).ThenInclude(_ => _.FireArmDeadlyForceAddendum)
                        .Include(_ => _.Responses).ThenInclude(_ => _.OtherDeadlyForceAddendum)
                        .Include(_ => _.Responses).ThenInclude(_ => _.PitUsageAddendum)
                        .Include(_ => _.Responses).ThenInclude(_ => _.TaserUsageAddendum)
                        .ThenInclude(_ => _.BodyUsageLocations)
                        .FirstOrDefaultAsync(_ =>
                                _.IncidentId.Equals(request.IncidentId) &&
                                _.SubjectId.Equals(request.SubjectId) &&
                                _.OfficerId.Equals(request.OfficerId),
                            cancellationToken);

                
                report.ApprovalStatus = ApprovalStatus.Created;

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

                if (report.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    report.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    report.TrainingApproverId = request.SubmitterId;
                } else {
                    report.SubmitterId = request.SubmitterId;
                }

                // 1. Remove Removed Resistances
                foreach (var resistance in report.Resistances) {

                    if (!request.Resistances.Any(_ => _.Resistance.Equals(resistance.ResistanceType))) {
                        _resistances.Remove(resistance);
                    }

                }

                //2. Add or Update Added Resistances
                foreach (var resistance in request.Resistances) {

                    if (!report.Resistances.Any(_ => _.ResistanceType.Equals(resistance.Resistance))) {
                        report.Resistances.Add(resistance.AsResistance());
                    } else {
                        var existingResistance =
                            report.Resistances.FirstOrDefault(_ => _.ResistanceType.Equals(resistance.Resistance));
                        if (existingResistance != null) {
                            existingResistance.Description = resistance.Description;
                        }
                    }

                }

                //3. Remove Removed Responses
                foreach (var response in report.Responses) {

                    if (!request.Responses.Any(_ => _.ResponseId.Equals(response.Id))) {
                        _responses.Remove(response);
                    }

                }

                //4. Add or Update Added Responses
                foreach (var response in request.Responses) {

                    if (!report.Responses.Any(_ => _.Id.Equals(response.ResponseId))) {

                        report.Responses.Add(response.AsResponse());

                    } else {

                        var existingResponse = report.Responses.FirstOrDefault(_ => _.Id.Equals(response.ResponseId));

                        if (existingResponse != null) {

                            existingResponse.WasEffective = response.WasEffective;

                            if (existingResponse.FireArmDeadlyForceAddendum != null) {

                                existingResponse.FireArmDeadlyForceAddendum.FireArmMake =
                                    response.FireArmDeadlyForceAddendum.FireArmMake;
                                existingResponse.FireArmDeadlyForceAddendum.FireArmModel =
                                    response.FireArmDeadlyForceAddendum.FireArmModel;
                                existingResponse.FireArmDeadlyForceAddendum.FireArmSerialNumber =
                                    response.FireArmDeadlyForceAddendum.FireArmSerialNumber;
                                existingResponse.FireArmDeadlyForceAddendum.FireArmAmmoType =
                                    response.FireArmDeadlyForceAddendum.FireArmAmmoType;

                            }

                            if (existingResponse.OtherDeadlyForceAddendum != null) {

                                existingResponse.OtherDeadlyForceAddendum.OtherDeadlyForceDescription =
                                    response.OtherDeadlyForceAddendum.OtherDeadlyForceDescription;

                            }

                            if (existingResponse.PitUsageAddendum != null) {

                                existingResponse.PitUsageAddendum.PitUsageVehicleSpeed =
                                    response.PitUsageAddendum.PitUsageVehicleSpeed;
                                existingResponse.PitUsageAddendum.WasSuspectVehicleImmobilized =
                                    response.PitUsageAddendum.WasSuspectVehicleImmobilized;
                                existingResponse.PitUsageAddendum.WasSecondaryImpactBySuspectVehicleAfterPit =
                                    response.PitUsageAddendum.WasSecondaryImpactBySuspectVehicleAfterPit;
                                existingResponse.PitUsageAddendum.SecondaryImpactBySuspectVehicleAfterPitPartsImpacted =
                                    response.PitUsageAddendum.SecondaryImpactBySuspectVehicleAfterPitPartsImpacted;

                            }

                            if (existingResponse.TaserUsageAddendum != null) {

                                existingResponse.TaserUsageAddendum.WasLaserDisplayUsed =
                                    response.TaserUsageAddendum.WasLaserDisplayUsed;
                                existingResponse.TaserUsageAddendum.WasArcDisplayUsed =
                                    response.TaserUsageAddendum.WasArcDisplayUsed;
                                existingResponse.TaserUsageAddendum.WasDriveStunUsed =
                                    response.TaserUsageAddendum.WasDriveStunUsed;
                                existingResponse.TaserUsageAddendum.WasProbeDeployUsed =
                                    response.TaserUsageAddendum.WasProbeDeployUsed;
                                existingResponse.TaserUsageAddendum.TaserSerialNumber =
                                    response.TaserUsageAddendum.TaserSerialNumber;
                                existingResponse.TaserUsageAddendum.DidProbesContact =
                                    response.TaserUsageAddendum.DidProbesContact;
                                existingResponse.TaserUsageAddendum.CyclesApplied =
                                    response.TaserUsageAddendum.CyclesApplied;
                                existingResponse.TaserUsageAddendum.DistanceWhenLaunched =
                                    response.TaserUsageAddendum.DistanceWhenLaunched;
                                existingResponse.TaserUsageAddendum.DistanceBetweenProbes =
                                    response.TaserUsageAddendum.DistanceBetweenProbes;
                                existingResponse.TaserUsageAddendum.AdditionalShotsRequired =
                                    response.TaserUsageAddendum.AdditionalShotsRequired;
                                existingResponse.TaserUsageAddendum.SubjectWearingHeavyClothing =
                                    response.TaserUsageAddendum.SubjectWearingHeavyClothing;
                                existingResponse.TaserUsageAddendum.DidProbesPenetrateSkin =
                                    response.TaserUsageAddendum.DidProbesPenetrateSkin;
                                existingResponse.TaserUsageAddendum.WereProbesRemovedAtScene =
                                    response.TaserUsageAddendum.WereProbesRemovedAtScene;
                                existingResponse.TaserUsageAddendum.WhoRemovedProbes =
                                    response.TaserUsageAddendum.WhoRemovedProbes;
                                existingResponse.TaserUsageAddendum.AnySecondaryInjuriesFromTaserUsage =
                                    response.TaserUsageAddendum.AnySecondaryInjuriesFromTaserUsage;
                                existingResponse.TaserUsageAddendum.WasMedicalAttentionRequiredForSecondaryInjuries =
                                    response.TaserUsageAddendum.WasMedicalAttentionRequiredForSecondaryInjuries;
                                existingResponse.TaserUsageAddendum.NumberOfPhotosTaken =
                                    response.TaserUsageAddendum.NumberOfPhotosTaken;
                                existingResponse.TaserUsageAddendum.CameraUsedToTakePhotos =
                                    response.TaserUsageAddendum.CameraUsedToTakePhotos;
                                existingResponse.TaserUsageAddendum.TaserCartridgeNumberUsed =
                                    response.TaserUsageAddendum.TaserCartridgeNumberUsed;

                                existingResponse.TaserUsageAddendum.BodyUsageLocations = response.TaserUsageAddendum
                                    .BodyUsageLocations.Select(_ => _.AsBodyUsageLocation()).ToList();

                            }

                        }

                    }

                }

                return Unit.Value;

            }

        }

    }

}