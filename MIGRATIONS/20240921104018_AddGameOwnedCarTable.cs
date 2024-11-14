using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGameOwnedCarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopTable_GamesTable_GameID",
                table: "ShopTable");

            migrationBuilder.DropIndex(
                name: "IX_ShopTable_GameID",
                table: "ShopTable");

            migrationBuilder.DropColumn(
                name: "GameID",
                table: "ShopTable");

            migrationBuilder.CreateTable(
                name: "OwnedCarsTable",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    ShopId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedCarsTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OwnedCarsTable_GamesTable_GameId",
                        column: x => x.GameId,
                        principalTable: "GamesTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnedCarsTable_ShopTable_ShopId",
                        column: x => x.ShopId,
                        principalTable: "ShopTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedCarsTable_GameId_ShopId",
                table: "OwnedCarsTable",
                columns: new[] { "GameId", "ShopId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnedCarsTable_ShopId",
                table: "OwnedCarsTable",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnedCarsTable");

            migrationBuilder.AddColumn<long>(
                name: "GameID",
                table: "ShopTable",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopTable_GameID",
                table: "ShopTable",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopTable_GamesTable_GameID",
                table: "ShopTable",
                column: "GameID",
                principalTable: "GamesTable",
                principalColumn: "ID");
        }
    }
}
