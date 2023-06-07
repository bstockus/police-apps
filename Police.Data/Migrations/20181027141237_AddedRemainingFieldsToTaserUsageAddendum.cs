using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AddedRemainingFieldsToTaserUsageAddendum : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int>(
                name: "AdditionalShotsRequired",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnySecondaryInjuriesFromTaserUsage",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CameraUsedToTakePhotos",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CyclesApplied",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DidProbesContact",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DidProbesPenetrateSkin",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DistanceBetweenProbes",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DistanceWhenLaunched",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPhotosTaken",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectWearingHeavyClothing",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaserCartridgeNumberUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "WasArcDisplayUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WasDriveStunUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WasLaserDisplayUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WasMedicalAttentionRequiredForSecondaryInjuries",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WasProbeDeployUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WereProbesRemovedAtScene",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WhoRemovedProbes",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "AdditionalShotsRequired",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "AnySecondaryInjuriesFromTaserUsage",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "CameraUsedToTakePhotos",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "CyclesApplied",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "DidProbesContact",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "DidProbesPenetrateSkin",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "DistanceBetweenProbes",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "DistanceWhenLaunched",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "NumberOfPhotosTaken",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "SubjectWearingHeavyClothing",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "TaserCartridgeNumberUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WasArcDisplayUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WasDriveStunUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WasLaserDisplayUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WasMedicalAttentionRequiredForSecondaryInjuries",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WasProbeDeployUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WereProbesRemovedAtScene",
                schema: "RestRep",
                table: "TaserUsageAddendums");

            migrationBuilder.DropColumn(
                name: "WhoRemovedProbes",
                schema: "RestRep",
                table: "TaserUsageAddendums");
        }
    }
}
