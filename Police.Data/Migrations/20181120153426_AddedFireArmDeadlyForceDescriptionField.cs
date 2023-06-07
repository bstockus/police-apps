using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AddedFireArmDeadlyForceDescriptionField : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                name: "FireArmDescription",
                schema: "RestRep",
                table: "FireArmDeadlyForceAddendums",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "FireArmDescription",
                schema: "RestRep",
                table: "FireArmDeadlyForceAddendums");
        }
    }
}
