using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class moldgrouping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoldGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAuto = table.Column<bool>(type: "bit", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoldGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoldGroupElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoldGroupId = table.Column<int>(type: "int", nullable: true),
                    FacilityId = table.Column<int>(type: "int", nullable: true),
                    MoldId = table.Column<int>(type: "int", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoldGroupElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoldGroupElements_Facilitys_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoldGroupElements_MoldGroups_MoldGroupId",
                        column: x => x.MoldGroupId,
                        principalTable: "MoldGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoldGroupElements_Molds_MoldId",
                        column: x => x.MoldId,
                        principalTable: "Molds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoldGroupElements_FacilityId",
                table: "MoldGroupElements",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldGroupElements_MoldGroupId",
                table: "MoldGroupElements",
                column: "MoldGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MoldGroupElements_MoldId",
                table: "MoldGroupElements",
                column: "MoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoldGroupElements");

            migrationBuilder.DropTable(
                name: "MoldGroups");
        }
    }
}
