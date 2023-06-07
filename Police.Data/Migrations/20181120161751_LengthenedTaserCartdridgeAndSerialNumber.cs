using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class LengthenedTaserCartdridgeAndSerialNumber : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                name: "TaserSerialNumber",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "TaserCartridgeNumberUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                name: "TaserSerialNumber",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TaserCartridgeNumberUsed",
                schema: "RestRep",
                table: "TaserUsageAddendums",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
