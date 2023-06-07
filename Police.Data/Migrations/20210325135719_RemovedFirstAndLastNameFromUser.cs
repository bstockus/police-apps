using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class RemovedFirstAndLastNameFromUser : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "SubjectAnimal");

            migrationBuilder.DropTable(
                name: "SubjectPerson");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "RestRep",
                table: "Subjects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DidSubjectRequireMedicalAttention",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DidSubjectRequireMedicalAttentionDescription",
                schema: "RestRep",
                table: "Subjects",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "RestRep",
                table: "Subjects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Race",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Species",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuspectedUse",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WasSubjectInjured",
                schema: "RestRep",
                table: "Subjects",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DidSubjectRequireMedicalAttention",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DidSubjectRequireMedicalAttentionDescription",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Race",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Species",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SuspectedUse",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "WasSubjectInjured",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SubjectAnimal",
                columns: table => new {
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Species = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_SubjectAnimal", x => new { x.IncidentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectAnimal_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectPerson",
                columns: table => new {
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DidSubjectRequireMedicalAttention = table.Column<int>(type: "int", nullable: true),
                    DidSubjectRequireMedicalAttentionDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Race = table.Column<int>(type: "int", nullable: true),
                    SuspectedUse = table.Column<int>(type: "int", nullable: true),
                    WasSubjectInjured = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_SubjectPerson", x => new { x.IncidentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectPerson_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }
    }
}
