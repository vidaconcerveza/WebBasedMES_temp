using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class molddraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoldDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoldId = table.Column<int>(type: "int", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoldDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoldDrafts_Molds_MoldId",
                        column: x => x.MoldId,
                        principalTable: "Molds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoldDraftId = table.Column<int>(type: "int", nullable: true),
                    UploadFileId = table.Column<int>(type: "int", nullable: true),
                    Classify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistererId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drafts_AspNetUsers_RegistererId",
                        column: x => x.RegistererId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drafts_MoldDrafts_MoldDraftId",
                        column: x => x.MoldDraftId,
                        principalTable: "MoldDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drafts_UploadFiles_UploadFileId",
                        column: x => x.UploadFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_MoldDraftId",
                table: "Drafts",
                column: "MoldDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_RegistererId",
                table: "Drafts",
                column: "RegistererId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_UploadFileId",
                table: "Drafts",
                column: "UploadFileId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldDrafts_MoldId",
                table: "MoldDrafts",
                column: "MoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drafts");

            migrationBuilder.DropTable(
                name: "MoldDrafts");
        }
    }
}
