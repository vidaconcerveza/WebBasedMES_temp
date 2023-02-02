using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class moldmanage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoldId",
                table: "UploadFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Molds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonCodeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Standard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    MoldCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwenerId = table.Column<int>(type: "int", nullable: true),
                    RegistererId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GuranteeCount = table.Column<int>(type: "int", nullable: false),
                    StartCount = table.Column<int>(type: "int", nullable: false),
                    CurrentCount = table.Column<int>(type: "int", nullable: false),
                    CleaningCycleId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UploadFileId = table.Column<int>(type: "int", nullable: true),
                    DailyInspection = table.Column<bool>(type: "bit", nullable: false),
                    DailyInspectionThreshold = table.Column<int>(type: "int", nullable: false),
                    RegularInspection = table.Column<bool>(type: "bit", nullable: false),
                    RegularInspectionThreshold = table.Column<int>(type: "int", nullable: false),
                    DiscardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscardRegistererId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscardReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Molds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Molds_AspNetUsers_DiscardRegistererId",
                        column: x => x.DiscardRegistererId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Molds_AspNetUsers_RegistererId",
                        column: x => x.RegistererId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Molds_CommonCodes_CleaningCycleId",
                        column: x => x.CleaningCycleId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Molds_CommonCodes_CommonCodeId",
                        column: x => x.CommonCodeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Molds_Partners_OwenerId",
                        column: x => x.OwenerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Molds_UploadFiles_UploadFileId",
                        column: x => x.UploadFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_MoldId",
                table: "UploadFiles",
                column: "MoldId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_CleaningCycleId",
                table: "Molds",
                column: "CleaningCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_CommonCodeId",
                table: "Molds",
                column: "CommonCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_DiscardRegistererId",
                table: "Molds",
                column: "DiscardRegistererId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_OwenerId",
                table: "Molds",
                column: "OwenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_RegistererId",
                table: "Molds",
                column: "RegistererId");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_UploadFileId",
                table: "Molds",
                column: "UploadFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Molds_MoldId",
                table: "UploadFiles",
                column: "MoldId",
                principalTable: "Molds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Molds_MoldId",
                table: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Molds");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_MoldId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "MoldId",
                table: "UploadFiles");
        }
    }
}
