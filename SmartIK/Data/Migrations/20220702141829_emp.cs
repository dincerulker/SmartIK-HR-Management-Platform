using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartIK.Data.Migrations
{
    public partial class emp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Numberofemployees",
                table: "Corporations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numberofemployees",
                table: "Corporations");
        }
    }
}
