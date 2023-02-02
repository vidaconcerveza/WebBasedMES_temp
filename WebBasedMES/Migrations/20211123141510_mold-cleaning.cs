using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class moldcleaning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoldCleanings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoldId = table.Column<int>(type: "int", nullable: true),
                    CurrentCleaningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoldCleanings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoldCleanings_Molds_MoldId",
                        column: x => x.MoldId,
                        principalTable: "Molds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoldCleaningElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoldCleaningId = table.Column<int>(type: "int", nullable: true),
                    CleaningClassifyId = table.Column<int>(type: "int", nullable: true),
                    CleaningTypeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CleaningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuranteeCount = table.Column<int>(type: "int", nullable: false),
                    StartCount = table.Column<int>(type: "int", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoldCleaningElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoldCleaningElements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoldCleaningElements_CommonCodes_CleaningClassifyId",
                        column: x => x.CleaningClassifyId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoldCleaningElements_CommonCodes_CleaningTypeId",
                        column: x => x.CleaningTypeId,
                        principalTable: "CommonCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoldCleaningElements_MoldCleanings_MoldCleaningId",
                        column: x => x.MoldCleaningId,
                        principalTable: "MoldCleanings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoldCleaningElements_CleaningClassifyId",
                table: "MoldCleaningElements",
                column: "CleaningClassifyId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldCleaningElements_CleaningTypeId",
                table: "MoldCleaningElements",
                column: "CleaningTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldCleaningElements_MoldCleaningId",
                table: "MoldCleaningElements",
                column: "MoldCleaningId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldCleaningElements_UserId",
                table: "MoldCleaningElements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldCleanings_MoldId",
                table: "MoldCleanings",
                column: "MoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoldCleaningElements");

            migrationBuilder.DropTable(
                name: "MoldCleanings");
        }
    }
}
