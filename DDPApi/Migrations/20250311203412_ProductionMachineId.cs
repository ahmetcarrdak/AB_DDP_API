using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductionMachineId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "ProductionInstructions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "ProductionInstructions");
        }
    }
}
