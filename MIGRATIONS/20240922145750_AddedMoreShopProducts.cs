using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreShopProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "GamesTable",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "ShopTable",
                columns: new[] { "ID", "Buyable", "CarType", "Cost", "ProductName" },
                values: new object[,]
                {
                    { 3L, true, 2, 125, "Ferrari" },
                    { 4L, true, 3, 400, "Lamborghini" },
                    { 5L, true, 1, 75, "Jeep" },
                    { 6L, true, 3, 250, "Rover" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 6L);

            migrationBuilder.AlterColumn<long>(
                name: "Currency",
                table: "GamesTable",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
