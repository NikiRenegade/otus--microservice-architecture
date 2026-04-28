using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class notUnicAvailableQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks",
                column: "AvailableQuantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_AvailableQuantity",
                table: "ProductStocks",
                column: "AvailableQuantity",
                unique: true);
        }
    }
}
