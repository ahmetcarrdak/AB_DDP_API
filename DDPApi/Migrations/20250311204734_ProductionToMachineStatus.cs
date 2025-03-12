using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductionToMachineStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProductionToMachines",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductionToMachines");
        }
    }
}
