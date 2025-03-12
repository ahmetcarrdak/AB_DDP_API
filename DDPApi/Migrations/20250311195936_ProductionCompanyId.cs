using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDPApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductionCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProductionInstructions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProductionInstructions");
        }
    }
}
