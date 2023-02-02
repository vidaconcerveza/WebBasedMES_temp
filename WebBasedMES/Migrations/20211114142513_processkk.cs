using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class processkk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDefective_Defectives_DefectiveId",
                table: "ProcessDefective");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDefective_Processes_ProcessId",
                table: "ProcessDefective");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDownTimeType_CommonCodes_DownTimeId",
                table: "ProcessDownTimeType");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDownTimeType_Processes_ProcessId",
                table: "ProcessDownTimeType");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessFacility_Facilitys_FacilityId",
                table: "ProcessFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessFacility_Processes_ProcessId",
                table: "ProcessFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessFacility",
                table: "ProcessFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessDownTimeType",
                table: "ProcessDownTimeType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessDefective",
                table: "ProcessDefective");

            migrationBuilder.RenameTable(
                name: "ProcessFacility",
                newName: "ProcessFacilities");

            migrationBuilder.RenameTable(
                name: "ProcessDownTimeType",
                newName: "ProcessDownTimeTypes");

            migrationBuilder.RenameTable(
                name: "ProcessDefective",
                newName: "ProcessDefectives");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessFacility_ProcessId",
                table: "ProcessFacilities",
                newName: "IX_ProcessFacilities_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessFacility_FacilityId",
                table: "ProcessFacilities",
                newName: "IX_ProcessFacilities_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDownTimeType_ProcessId",
                table: "ProcessDownTimeTypes",
                newName: "IX_ProcessDownTimeTypes_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDownTimeType_DownTimeId",
                table: "ProcessDownTimeTypes",
                newName: "IX_ProcessDownTimeTypes_DownTimeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDefective_ProcessId",
                table: "ProcessDefectives",
                newName: "IX_ProcessDefectives_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDefective_DefectiveId",
                table: "ProcessDefectives",
                newName: "IX_ProcessDefectives_DefectiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessFacilities",
                table: "ProcessFacilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessDownTimeTypes",
                table: "ProcessDownTimeTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessDefectives",
                table: "ProcessDefectives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDefectives_Defectives_DefectiveId",
                table: "ProcessDefectives",
                column: "DefectiveId",
                principalTable: "Defectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDefectives_Processes_ProcessId",
                table: "ProcessDefectives",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDownTimeTypes_CommonCodes_DownTimeId",
                table: "ProcessDownTimeTypes",
                column: "DownTimeId",
                principalTable: "CommonCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDownTimeTypes_Processes_ProcessId",
                table: "ProcessDownTimeTypes",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessFacilities_Facilitys_FacilityId",
                table: "ProcessFacilities",
                column: "FacilityId",
                principalTable: "Facilitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessFacilities_Processes_ProcessId",
                table: "ProcessFacilities",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDefectives_Defectives_DefectiveId",
                table: "ProcessDefectives");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDefectives_Processes_ProcessId",
                table: "ProcessDefectives");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDownTimeTypes_CommonCodes_DownTimeId",
                table: "ProcessDownTimeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDownTimeTypes_Processes_ProcessId",
                table: "ProcessDownTimeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessFacilities_Facilitys_FacilityId",
                table: "ProcessFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessFacilities_Processes_ProcessId",
                table: "ProcessFacilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessFacilities",
                table: "ProcessFacilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessDownTimeTypes",
                table: "ProcessDownTimeTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessDefectives",
                table: "ProcessDefectives");

            migrationBuilder.RenameTable(
                name: "ProcessFacilities",
                newName: "ProcessFacility");

            migrationBuilder.RenameTable(
                name: "ProcessDownTimeTypes",
                newName: "ProcessDownTimeType");

            migrationBuilder.RenameTable(
                name: "ProcessDefectives",
                newName: "ProcessDefective");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessFacilities_ProcessId",
                table: "ProcessFacility",
                newName: "IX_ProcessFacility_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessFacilities_FacilityId",
                table: "ProcessFacility",
                newName: "IX_ProcessFacility_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDownTimeTypes_ProcessId",
                table: "ProcessDownTimeType",
                newName: "IX_ProcessDownTimeType_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDownTimeTypes_DownTimeId",
                table: "ProcessDownTimeType",
                newName: "IX_ProcessDownTimeType_DownTimeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDefectives_ProcessId",
                table: "ProcessDefective",
                newName: "IX_ProcessDefective_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessDefectives_DefectiveId",
                table: "ProcessDefective",
                newName: "IX_ProcessDefective_DefectiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessFacility",
                table: "ProcessFacility",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessDownTimeType",
                table: "ProcessDownTimeType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessDefective",
                table: "ProcessDefective",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDefective_Defectives_DefectiveId",
                table: "ProcessDefective",
                column: "DefectiveId",
                principalTable: "Defectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDefective_Processes_ProcessId",
                table: "ProcessDefective",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDownTimeType_CommonCodes_DownTimeId",
                table: "ProcessDownTimeType",
                column: "DownTimeId",
                principalTable: "CommonCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDownTimeType_Processes_ProcessId",
                table: "ProcessDownTimeType",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessFacility_Facilitys_FacilityId",
                table: "ProcessFacility",
                column: "FacilityId",
                principalTable: "Facilitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessFacility_Processes_ProcessId",
                table: "ProcessFacility",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
