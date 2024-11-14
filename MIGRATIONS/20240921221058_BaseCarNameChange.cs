using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class BaseCarNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Buyable",
                table: "ShopTable",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "Buyable", "ProductName" },
                values: new object[] { false, "Base car" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buyable",
                table: "ShopTable");

            migrationBuilder.UpdateData(
                table: "ShopTable",
                keyColumn: "ID",
                keyValue: 1L,
                column: "ProductName",
                value: "Base_car");
        }
    }
}
