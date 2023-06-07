using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class InitialCreate : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.EnsureSchema(
                name: "Org");

            migrationBuilder.EnsureSchema(
                name: "RestRep");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new {
                    Id = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 200, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 255, nullable: false),
                    WindowsSid = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_WindowsSid", x => x.WindowsSid);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "Identity",
                columns: table => new {
                    RoleId = table.Column<Guid>(nullable: false),
                    PermissionName = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionName });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Officers",
                schema: "Org",
                columns: table => new {
                    Id = table.Column<Guid>(nullable: false),
                    BadgeNumber = table.Column<string>(maxLength: 4, nullable: false),
                    EmployeeNumber = table.Column<string>(maxLength: 8, nullable: false),
                    Rank = table.Column<string>(maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 200, nullable: false),
                    Assignment = table.Column<string>(maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Officers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Officers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                schema: "RestRep",
                columns: table => new {
                    Id = table.Column<Guid>(nullable: false),
                    IncidentDateAndTime = table.Column<DateTime>(nullable: false),
                    IncidentCaseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    SubmitterId = table.Column<Guid>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentOfficers",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    WasOfficerInjured = table.Column<int>(nullable: false),
                    DidOfficerRequireMedicalAttention = table.Column<int>(nullable: false),
                    DidOfficerRequireMedicalAttentionDescription = table.Column<string>(maxLength: 1000, nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    SubmitterId = table.Column<Guid>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_IncidentOfficers", x => new { x.IncidentId, x.OfficerId });
                    table.ForeignKey(
                        name: "FK_IncidentOfficers_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentOfficers_Officers_OfficerId",
                        column: x => x.OfficerId,
                        principalSchema: "Org",
                        principalTable: "Officers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentOfficers_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    SubmitterId = table.Column<Guid>(nullable: false),
                    SubjectType = table.Column<string>(maxLength: 10, nullable: false),
                    Species = table.Column<int>(nullable: true),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Race = table.Column<int>(nullable: true),
                    SuspectedUse = table.Column<int>(nullable: true),
                    WasSubjectInjured = table.Column<int>(nullable: true),
                    DidSubjectRequireMedicalAttention = table.Column<int>(nullable: true),
                    DidSubjectRequireMedicalAttentionDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Subjects", x => new { x.IncidentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_Subjects_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subjects_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    SubmitterId = table.Column<Guid>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Reports", x => new { x.IncidentId, x.OfficerId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_Reports_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Officers_OfficerId",
                        column: x => x.OfficerId,
                        principalSchema: "Org",
                        principalTable: "Officers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_IncidentOfficers_IncidentId_OfficerId",
                        columns: x => new { x.IncidentId, x.OfficerId },
                        principalSchema: "RestRep",
                        principalTable: "IncidentOfficers",
                        principalColumns: new[] { "IncidentId", "OfficerId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Subjects_IncidentId_SubjectId",
                        columns: x => new { x.IncidentId, x.SubjectId },
                        principalSchema: "RestRep",
                        principalTable: "Subjects",
                        principalColumns: new[] { "IncidentId", "SubjectId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resistances",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResistanceType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Resistances", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResistanceType });
                    table.ForeignKey(
                        name: "FK_Resistances_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resistances_Reports_IncidentId_OfficerId_SubjectId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId },
                        principalSchema: "RestRep",
                        principalTable: "Reports",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ResponseType = table.Column<int>(nullable: false),
                    WasEffective = table.Column<int>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Responses", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.Id });
                    table.ForeignKey(
                        name: "FK_Responses_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Responses_Reports_IncidentId_OfficerId_SubjectId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId },
                        principalSchema: "RestRep",
                        principalTable: "Reports",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FireArmDeadlyForceAddendums",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResponseId = table.Column<Guid>(nullable: false),
                    FireArmMake = table.Column<string>(maxLength: 100, nullable: false),
                    FireArmModel = table.Column<string>(maxLength: 100, nullable: false),
                    FireArmSerialNumber = table.Column<string>(maxLength: 100, nullable: false),
                    FireArmAmmoType = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_FireArmDeadlyForceAddendums", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId });
                    table.ForeignKey(
                        name: "FK_FireArmDeadlyForceAddendums_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FireArmDeadlyForceAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId },
                        principalSchema: "RestRep",
                        principalTable: "Responses",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherDeadlyForceAddendums",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResponseId = table.Column<Guid>(nullable: false),
                    OtherDeadlyForceDescription = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_OtherDeadlyForceAddendums", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId });
                    table.ForeignKey(
                        name: "FK_OtherDeadlyForceAddendums_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OtherDeadlyForceAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId },
                        principalSchema: "RestRep",
                        principalTable: "Responses",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PitUsageAddendums",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResponseId = table.Column<Guid>(nullable: false),
                    PitUsageVehicleSpeed = table.Column<int>(nullable: false),
                    WasSuspectVehicleImmobilized = table.Column<int>(nullable: false),
                    WasSecondaryImpactBySuspectVehicleAfterPit = table.Column<int>(nullable: false),
                    SecondaryImpactBySuspectVehicleAfterPitPartsImpacted = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PitUsageAddendums", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId });
                    table.ForeignKey(
                        name: "FK_PitUsageAddendums_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PitUsageAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId },
                        principalSchema: "RestRep",
                        principalTable: "Responses",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Officers_UserId",
                schema: "Org",
                table: "Officers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentOfficers_OfficerId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentOfficers_SubmitterId",
                schema: "RestRep",
                table: "IncidentOfficers",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_SubmitterId",
                schema: "RestRep",
                table: "Incidents",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OfficerId",
                schema: "RestRep",
                table: "Reports",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SubmitterId",
                schema: "RestRep",
                table: "Reports",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_IncidentId_SubjectId",
                schema: "RestRep",
                table: "Reports",
                columns: new[] { "IncidentId", "SubjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubmitterId",
                schema: "RestRep",
                table: "Subjects",
                column: "SubmitterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "FireArmDeadlyForceAddendums",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "OtherDeadlyForceAddendums",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "PitUsageAddendums",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Resistances",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Responses",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "IncidentOfficers",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Officers",
                schema: "Org");

            migrationBuilder.DropTable(
                name: "Incidents",
                schema: "RestRep");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");
        }
    }
}
