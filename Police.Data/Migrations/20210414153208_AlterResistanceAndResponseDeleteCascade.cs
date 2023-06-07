using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AlterResistanceAndResponseDeleteCascade : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Resistances_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Resistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Responses");

            migrationBuilder.AddForeignKey(
                name: "FK_Resistances_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Resistances",
                columns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                principalSchema: "RestRep",
                principalTable: "Reports",
                principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Responses",
                columns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                principalSchema: "RestRep",
                principalTable: "Reports",
                principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Resistances_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Resistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Responses");

            migrationBuilder.AddForeignKey(
                name: "FK_Resistances_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Resistances",
                columns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                principalSchema: "RestRep",
                principalTable: "Reports",
                principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Reports_IncidentId_OfficerId_SubjectId",
                schema: "RestRep",
                table: "Responses",
                columns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                principalSchema: "RestRep",
                principalTable: "Reports",
                principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
