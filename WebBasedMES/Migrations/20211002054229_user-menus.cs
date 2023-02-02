using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class usermenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAbleToRead",
                table: "Submenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbleToReadWrite",
                table: "Submenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Submenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbleToRead",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbleToReadWrite",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAbleToRead",
                table: "Submenus");

            migrationBuilder.DropColumn(
                name: "IsAbleToReadWrite",
                table: "Submenus");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Submenus");

            migrationBuilder.DropColumn(
                name: "IsAbleToRead",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IsAbleToReadWrite",
                table: "Menus");
        }
    }
}
