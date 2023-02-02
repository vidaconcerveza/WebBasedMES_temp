using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class faciilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilitys_Partners_AgentId",
                table: "Facilitys");

            migrationBuilder.DropIndex(
                name: "IX_Facilitys_AgentId",
                table: "Facilitys");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Facilitys");

            migrationBuilder.AddColumn<string>(
                name: "Agent",
                table: "Facilitys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "Facilitys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agent",
                table: "Facilitys");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "Facilitys");

            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Facilitys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facilitys_AgentId",
                table: "Facilitys",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilitys_Partners_AgentId",
                table: "Facilitys",
                column: "AgentId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
