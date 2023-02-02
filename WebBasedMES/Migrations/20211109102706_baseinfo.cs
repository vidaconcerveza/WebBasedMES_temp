using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class baseinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "UploadFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Defectives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defectives_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Defectives_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facilitys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Standard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    MaxCurrent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxTon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyInspection = table.Column<bool>(type: "bit", nullable: false),
                    PeriodicalInspection = table.Column<bool>(type: "bit", nullable: false),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UploadFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilitys_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facilitys_Partners_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facilitys_UploadFiles_UploadFileId",
                        column: x => x.UploadFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionCount = table.Column<int>(type: "int", nullable: false),
                    InspectionItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JudgeStandard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JudgeMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionItems_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionItems_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    InspectionItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionStandard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionTypes_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_FacilityId",
                table: "UploadFiles",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Defectives_CommonCodeId",
                table: "Defectives",
                column: "CommonCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Defectives_CreatorId",
                table: "Defectives",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilitys_AgentId",
                table: "Facilitys",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilitys_CommonCodeId",
                table: "Facilitys",
                column: "CommonCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilitys_UploadFileId",
                table: "Facilitys",
                column: "UploadFileId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_CommonCodeId",
                table: "InspectionItems",
                column: "CommonCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_CreatorId",
                table: "InspectionItems",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionTypes_CommonCodeId",
                table: "InspectionTypes",
                column: "CommonCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Facilitys_FacilityId",
                table: "UploadFiles",
                column: "FacilityId",
                principalTable: "Facilitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Facilitys_FacilityId",
                table: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Defectives");

            migrationBuilder.DropTable(
                name: "Facilitys");

            migrationBuilder.DropTable(
                name: "InspectionItems");

            migrationBuilder.DropTable(
                name: "InspectionTypes");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_FacilityId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "UploadFiles");
        }
    }
}
