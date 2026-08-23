using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POSSystem2.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Sku", "Category", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { "P001", "Apparel", "Blue T-Shirt", 19.99m, 100 },
                    { "P002", "Home", "Coffee Mug", 7.5m, 200 },
                    { "P003", "Office", "Notebook", 3.25m, 500 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Sku",
                keyValue: "P001");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Sku",
                keyValue: "P002");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Sku",
                keyValue: "P003");
        }
    }
}
