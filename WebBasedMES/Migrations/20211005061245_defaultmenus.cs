using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class defaultmenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAbleToAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsAbleToRead = table.Column<bool>(type: "bit", nullable: false),
                    IsAbleToReadWrite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DefaultSubmenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultMenuId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAbleToAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsAbleToRead = table.Column<bool>(type: "bit", nullable: false),
                    IsAbleToReadWrite = table.Column<bool>(type: "bit", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultSubmenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultSubmenus_DefaultMenus_DefaultMenuId",
                        column: x => x.DefaultMenuId,
                        principalTable: "DefaultMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultSubmenus_DefaultMenuId",
                table: "DefaultSubmenus",
                column: "DefaultMenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultSubmenus");

            migrationBuilder.DropTable(
                name: "DefaultMenus");
        }
    }
}
