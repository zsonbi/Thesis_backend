using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class SportWhiteCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ShopTable",
                columns: new[] { "ID", "Buyable", "CarType", "Cost", "ProductName" },
                values: new object[] { 2L, true, 1, 50, "Sport white car" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 2L);
        }
    }
}
