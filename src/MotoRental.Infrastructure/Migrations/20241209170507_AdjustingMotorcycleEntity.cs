using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingMotorcycleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Motorcycles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Motorcycles");
        }
    }
}
