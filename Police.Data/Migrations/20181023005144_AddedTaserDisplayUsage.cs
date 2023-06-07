using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    public partial class AddedTaserDisplayUsage : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "TaserDisplayUsageAddendums",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResponseId = table.Column<Guid>(nullable: false),
                    TaserSerialNumber = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TaserDisplayUsageAddendums", x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId });
                    table.ForeignKey(
                        name: "FK_TaserDisplayUsageAddendums_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaserDisplayUsageAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId",
                        columns: x => new { x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId },
                        principalSchema: "RestRep",
                        principalTable: "Responses",
                        principalColumns: new[] { "IncidentId", "OfficerId", "SubjectId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "TaserDisplayUsageAddendums",
                schema: "RestRep");
        }
    }
}
