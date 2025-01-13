using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Works",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StationType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false),
                    MaxWorkerCount = table.Column<int>(type: "integer", nullable: false),
                    AverageProcessTime = table.Column<int>(type: "integer", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    ResponsiblePersonId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    SpecialNotes = table.Column<string>(type: "text", nullable: false),
                    RequiresQualityCheck = table.Column<bool>(type: "boolean", nullable: false),
                    MaintenanceRequired = table.Column<bool>(type: "boolean", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.StationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_StationId",
                table: "Works",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StationId",
                table: "Orders",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stations_StationId",
                table: "Orders",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Stations_StationId",
                table: "Works",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_StationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Stations_StationId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Works_StationId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Orders");
        }
    }
}
