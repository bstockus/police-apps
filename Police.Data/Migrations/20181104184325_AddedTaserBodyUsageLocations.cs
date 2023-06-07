using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {

    public partial class AddedTaserBodyUsageLocations : Migration {

        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep",
                columns: table => new {
                    IncidentId = table.Column<Guid>(nullable: false),
                    OfficerId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    ResponseId = table.Column<Guid>(nullable: false),
                    BodyUsageType = table.Column<int>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TaserUsageAddendums.BodyUsageLocations",
                        x => new {x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId});
                    table.ForeignKey(
                        name: "FK_TaserUsageAddendums.BodyUsageLocations_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalSchema: "RestRep",
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name:
                        "FK_TaserUsageAddendums.BodyUsageLocations_TaserUsageAddendums_IncidentId_OfficerId_SubjectId_ResponseId",
                        columns: x => new {x.IncidentId, x.OfficerId, x.SubjectId, x.ResponseId},
                        principalSchema: "RestRep",
                        principalTable: "TaserUsageAddendums",
                        principalColumns: new[] {"IncidentId", "OfficerId", "SubjectId", "ResponseId"},
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep");
        }

    }

}