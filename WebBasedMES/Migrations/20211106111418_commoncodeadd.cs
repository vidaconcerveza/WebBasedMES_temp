using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class commoncodeadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "CommonCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "CommonCodes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CommonCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CommonCodes_CreatorId",
                table: "CommonCodes",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonCodes_AspNetUsers_CreatorId",
                table: "CommonCodes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonCodes_AspNetUsers_CreatorId",
                table: "CommonCodes");

            migrationBuilder.DropIndex(
                name: "IX_CommonCodes_CreatorId",
                table: "CommonCodes");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "CommonCodes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CommonCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CommonCodes");
        }
    }
}
