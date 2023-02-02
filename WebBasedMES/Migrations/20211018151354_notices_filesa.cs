using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class notices_filesa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFile_Notices_NoticeId",
                table: "UploadFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadFile",
                table: "UploadFile");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UploadFile");

            migrationBuilder.RenameTable(
                name: "UploadFile",
                newName: "UploadFiles");

            migrationBuilder.RenameIndex(
                name: "IX_UploadFile_NoticeId",
                table: "UploadFiles",
                newName: "IX_UploadFiles_NoticeId");

            migrationBuilder.AddColumn<string>(
                name: "FileStamp",
                table: "UploadFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadFiles",
                table: "UploadFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Notices_NoticeId",
                table: "UploadFiles",
                column: "NoticeId",
                principalTable: "Notices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Notices_NoticeId",
                table: "UploadFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadFiles",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "FileStamp",
                table: "UploadFiles");

            migrationBuilder.RenameTable(
                name: "UploadFiles",
                newName: "UploadFile");

            migrationBuilder.RenameIndex(
                name: "IX_UploadFiles_NoticeId",
                table: "UploadFile",
                newName: "IX_UploadFile_NoticeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UploadFile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadFile",
                table: "UploadFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFile_Notices_NoticeId",
                table: "UploadFile",
                column: "NoticeId",
                principalTable: "Notices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
