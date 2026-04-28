using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class notUnicAvailableQuantity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks",
                column: "AvailableQuantity");
        }
    }
}
