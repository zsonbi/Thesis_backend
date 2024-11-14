using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class addedNewCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 6L,
                column: "CarType",
                value: 2);

            migrationBuilder.InsertData(
                table: "ShopTable",
                columns: new[] { "ID", "Buyable", "CarType", "Cost", "ProductName" },
                values: new object[,]
                {
                    { 7L, true, 3, 450, "Forma1" },
                    { 8L, true, 1, 100, "Ambulance" },
                    { 9L, true, 0, 25, "Taxi" },
                    { 10L, true, 0, 40, "Van" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 10L);

            migrationBuilder.UpdateData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 6L,
                column: "CarType",
                value: 3);
        }
    }
}
