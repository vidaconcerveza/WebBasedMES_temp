using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class process : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityUse = table.Column<bool>(type: "bit", nullable: false),
                    ProcessInspection = table.Column<bool>(type: "bit", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDefective",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefectiveId = table.Column<int>(type: "int", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDefective", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDefective_Defectives_DefectiveId",
                        column: x => x.DefectiveId,
                        principalTable: "Defectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDefective_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDownTimeType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DownTimeId = table.Column<int>(type: "int", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDownTimeType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDownTimeType_CommonCodes_DownTimeId",
                        column: x => x.DownTimeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDownTimeType_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessFacility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessFacility_Facilitys_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessFacility_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDefective_DefectiveId",
                table: "ProcessDefective",
                column: "DefectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDefective_ProcessId",
                table: "ProcessDefective",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDownTimeType_DownTimeId",
                table: "ProcessDownTimeType",
                column: "DownTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDownTimeType_ProcessId",
                table: "ProcessDownTimeType",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_CommonCodeId",
                table: "Processes",
                column: "CommonCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessFacility_FacilityId",
                table: "ProcessFacility",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessFacility_ProcessId",
                table: "ProcessFacility",
                column: "ProcessId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessDefective");

            migrationBuilder.DropTable(
                name: "ProcessDownTimeType");

            migrationBuilder.DropTable(
                name: "ProcessFacility");

            migrationBuilder.DropTable(
                name: "Processes");
        }
    }
}
