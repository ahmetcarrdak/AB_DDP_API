using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductionInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionInstructions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductionInstructionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Barkod = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionStores_ProductionInstructions_ProductionInstructi~",
                        column: x => x.ProductionInstructionId,
                        principalTable: "ProductionInstructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionToMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductionInstructionId = table.Column<int>(type: "integer", nullable: false),
                    MachineId = table.Column<int>(type: "integer", nullable: false),
                    Line = table.Column<int>(type: "integer", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionToMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionToMachines_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionToMachines_ProductionInstructions_ProductionInstr~",
                        column: x => x.ProductionInstructionId,
                        principalTable: "ProductionInstructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStores_ProductionInstructionId",
                table: "ProductionStores",
                column: "ProductionInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionToMachines_MachineId",
                table: "ProductionToMachines",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionToMachines_ProductionInstructionId",
                table: "ProductionToMachines",
                column: "ProductionInstructionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionStores");

            migrationBuilder.DropTable(
                name: "ProductionToMachines");

            migrationBuilder.DropTable(
                name: "ProductionInstructions");
        }
    }
}
