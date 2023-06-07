using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {

    public partial class ModifiedTaserBodyUsageLocationsPrimaryKey : Migration {

        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep",
                table: "TaserUsageAddendums.BodyUsageLocations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep",
                table: "TaserUsageAddendums.BodyUsageLocations",
                columns: new[] {"IncidentId", "OfficerId", "SubjectId", "ResponseId", "BodyUsageType", "X", "Y"});
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep",
                table: "TaserUsageAddendums.BodyUsageLocations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaserUsageAddendums.BodyUsageLocations",
                schema: "RestRep",
                table: "TaserUsageAddendums.BodyUsageLocations",
                columns: new[] {"IncidentId", "OfficerId", "SubjectId", "ResponseId"});
        }

    }

}