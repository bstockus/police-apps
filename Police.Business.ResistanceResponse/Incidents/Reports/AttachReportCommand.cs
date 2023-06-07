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
using Police.Business.ResistanceResponse.Incidents.IncidentOfficers;
using Police.Business.ResistanceResponse.Incidents.Reports.Resistances;
using Police.Business.ResistanceResponse.Incidents.Reports.Responses;
using Police.Business.ResistanceResponse.Incidents.Subjects;
using Police.Data;
using Police.Security.User;

namespace Police.Business.ResistanceResponse.Incidents.Reports {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class AttachReportCommand : IRequest {

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
                        FireArmAmmoType =  FireArmAmmoType,
                        FireArmDescription = ""
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

        public class Validator : AbstractValidator<AttachReportCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
                RuleFor(_ => _.OfficerId).AsEntityIdentity();
                RuleFor(_ => _.SubmitterId).AsEntityIdentity();
                RuleForEach(_ => _.Resistances).SetValidator(new ResistanceData.ResistanceDataValidator());
                RuleForEach(_ => _.Responses).SetValidator(new ResponseData.ResponseDataValidator());
            }

        }

        public class Handler : IRequestHandler<AttachReportCommand> {

            private readonly DbSet<Incident> _incidents;
            private readonly DbSet<Subject> _subjects;
            private readonly DbSet<IncidentOfficer> _incidentOfficers;
            private readonly DbSet<Report> _reports;
            private readonly DbSet<User> _users;
            private readonly IUserService _userService;

            public Handler(
                DbSet<Incident> incidents,
                DbSet<Subject> subjects,
                DbSet<IncidentOfficer> incidentOfficers,
                DbSet<Report> reports,
                DbSet<User> users,
                IUserService userService) {
                _incidents = incidents;
                _subjects = subjects;
                _incidentOfficers = incidentOfficers;
                _reports = reports;
                _users = users;
                _userService = userService;
            }

            public async Task<Unit> Handle(AttachReportCommand request, CancellationToken cancellationToken) {

                await _incidents.ThrowIfIncidentDoesNotExist(request.IncidentId, cancellationToken);
                await _users.ThrowIfUserDoesNotExist(request.SubmitterId, cancellationToken);
                await _incidentOfficers.ThrowIfIncidentOfficerDoesNotExist(request.IncidentId, request.OfficerId,
                    cancellationToken);
                await _subjects.ThrowIfSubjectDoesNotExist(request.IncidentId, request.SubjectId, cancellationToken);
                await _reports.ThrowIfReportExists(request.IncidentId, request.OfficerId, request.SubjectId,
                    cancellationToken);

                var user = await _userService.FetchUserInformationByUserId(request.SubmitterId);

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

                var report = new Report {
                    IncidentId = request.IncidentId,
                    OfficerId = request.OfficerId,
                    SubjectId = request.SubjectId,
                    SubmitterId = request.SubmitterId,
                    Resistances = request.Resistances.Select(_ => _.AsResistance()).ToList(),
                    Responses = request.Responses.Select(_ => _.AsResponse()).ToList(),
                    ApprovalStatus = ApprovalStatus.Created
                };

                if (report.AsApprovalInformation().IsUserAllowedToMakeChangesWithoutApproval(user)) {
                    report.ApprovalStatus = ApprovalStatus.ApprovedByTraining;
                    report.TrainingApproverId = request.SubmitterId;
                }

                _reports.Add(report);

                return Unit.Value;

            }

        }

    }

}