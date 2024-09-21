using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class shop_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "UsersTable");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "GamesTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ShopTable",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    CarType = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShopTable_GamesTable_GameID",
                        column: x => x.GameID,
                        principalTable: "GamesTable",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GamesTable_UserId",
                table: "GamesTable",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopTable_GameID",
                table: "ShopTable",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_GamesTable_UsersTable_UserId",
                table: "GamesTable",
                column: "UserId",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamesTable_UsersTable_UserId",
                table: "GamesTable");

            migrationBuilder.DropTable(
                name: "ShopTable");

            migrationBuilder.DropIndex(
                name: "IX_GamesTable_UserId",
                table: "GamesTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GamesTable");

            migrationBuilder.AddColumn<long>(
                name: "GameId",
                table: "UsersTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
