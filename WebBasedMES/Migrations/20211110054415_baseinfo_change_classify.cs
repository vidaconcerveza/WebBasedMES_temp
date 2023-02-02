using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBasedMES.Migrations
{
    public partial class baseinfo_change_classify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "InspectionItems",
                newName: "CreateDateTime");

            migrationBuilder.RenameColumn(
                name: "Class",
                table: "InspectionItems",
                newName: "Classify");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDateTime",
                table: "InspectionItems",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "Classify",
                table: "InspectionItems",
                newName: "Class");
        }
    }
}
