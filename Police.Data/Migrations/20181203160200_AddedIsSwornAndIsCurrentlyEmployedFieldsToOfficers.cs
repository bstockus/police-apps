using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AddedIsSwornAndIsCurrentlyEmployedFieldsToOfficers : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentlyEmployed",
                schema: "Org",
                table: "Officers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSwornOfficer",
                schema: "Org",
                table: "Officers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "IsCurrentlyEmployed",
                schema: "Org",
                table: "Officers");

            migrationBuilder.DropColumn(
                name: "IsSwornOfficer",
                schema: "Org",
                table: "Officers");
        }
    }
}
