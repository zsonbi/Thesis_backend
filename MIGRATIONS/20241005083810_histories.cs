using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class histories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameScoresTable",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    AchievedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameScoresTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameScoresTable_UsersTable_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "UsersTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskHistoriesTable",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompletedTaskID = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    Completed = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskHistoriesTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskHistoriesTable_TasksTable_CompletedTaskID",
                        column: x => x.CompletedTaskID,
                        principalTable: "TasksTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskHistoriesTable_UsersTable_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "UsersTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GameScoresTable_OwnerId",
                table: "GameScoresTable",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistoriesTable_CompletedTaskID",
                table: "TaskHistoriesTable",
                column: "CompletedTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistoriesTable_OwnerId",
                table: "TaskHistoriesTable",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameScoresTable");

            migrationBuilder.DropTable(
                name: "TaskHistoriesTable");
        }
    }
}
