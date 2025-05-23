using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassiveMoneyTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePassiveIncomeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "PassiveIncomes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "PassiveIncomes",
                newName: "Received");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PassiveIncomes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PassiveIncomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "PassiveIncomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PassiveIncomes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PassiveIncomes");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "PassiveIncomes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PassiveIncomes",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "Received",
                table: "PassiveIncomes",
                newName: "Month");
        }
    }
}
