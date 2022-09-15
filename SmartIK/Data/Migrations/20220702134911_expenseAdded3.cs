using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartIK.Data.Migrations
{
    public partial class expenseAdded3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseManager",
                table: "Expenses",
                newName: "Response");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseDate",
                table: "Expenses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Expenses",
                newName: "ResponseManager");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseDate",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
