using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AddedApprovalProcessRelatedFields : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Subjects",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Subjects",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Reports",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Reports",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Incidents",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Incidents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Incidents",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "IncidentOfficers",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "IncidentOfficers",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects",
                column: "SupervisorApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TrainingApproverId",
                schema: "RestRep",
                table: "Subjects",
                column: "TrainingApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SupervisorApproverId",
                schema: "RestRep",
                table: "Reports",
                column: "SupervisorApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TrainingApproverId",
                schema: "RestRep",
                table: "Reports",
                column: "TrainingApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents",
                column: "SupervisorApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_TrainingApproverId",
                schema: "RestRep",
                table: "Incidents",
                column: "TrainingApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentOfficers_SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "SupervisorApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentOfficers_TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "TrainingApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentOfficers_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "SupervisorApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentOfficers_Users_TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "TrainingApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents",
                column: "SupervisorApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Incidents",
                column: "TrainingApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Reports",
                column: "SupervisorApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Reports",
                column: "TrainingApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects",
                column: "SupervisorApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Subjects",
                column: "TrainingApproverId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentOfficers_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentOfficers_Users_TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_TrainingApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TrainingApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SupervisorApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TrainingApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_TrainingApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_IncidentOfficers_SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropIndex(
                name: "IX_IncidentOfficers_TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropColumn(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "SupervisorApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropColumn(
                name: "SupervisorsComments",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropColumn(
                name: "TrainingApproverId",
                schema: "RestRep",
                table: "IncidentOfficers");

            migrationBuilder.DropColumn(
                name: "TrainingsComments",
                schema: "RestRep",
                table: "IncidentOfficers");
        }
    }
}
