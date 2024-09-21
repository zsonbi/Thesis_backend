using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseCarToShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ShopTable",
                columns: new[] { "ID", "CarType", "Cost", "ProductName" },
                values: new object[] { 1L, 0, 0, "Base_car" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 1L);
        }
    }
}
