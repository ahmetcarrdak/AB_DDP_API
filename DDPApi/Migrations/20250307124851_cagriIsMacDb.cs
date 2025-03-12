using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class cagriIsMacDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Persons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Positions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Persons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Persons",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
