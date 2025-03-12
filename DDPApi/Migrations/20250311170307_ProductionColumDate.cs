using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductionColumDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ProductionInstructions",
                newName: "InsertDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ComplatedDate",
                table: "ProductionInstructions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ProductionInstructions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplatedDate",
                table: "ProductionInstructions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ProductionInstructions");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "ProductionInstructions",
                newName: "Date");
        }
    }
}
