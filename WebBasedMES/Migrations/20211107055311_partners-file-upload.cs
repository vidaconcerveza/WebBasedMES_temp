using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class partnersfileupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "UploadFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_PartnerId",
                table: "UploadFiles",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Partners_PartnerId",
                table: "UploadFiles",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Partners_PartnerId",
                table: "UploadFiles");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_PartnerId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "UploadFiles");
        }
    }
}
