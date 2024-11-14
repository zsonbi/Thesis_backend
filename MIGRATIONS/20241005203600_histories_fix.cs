using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class histories_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistoriesTable_TasksTable_CompletedTaskID",
                table: "TaskHistoriesTable");

            migrationBuilder.DropIndex(
                name: "IX_TaskHistoriesTable_CompletedTaskID",
                table: "TaskHistoriesTable");

            migrationBuilder.DropColumn(
                name: "CompletedTaskID",
                table: "TaskHistoriesTable");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistoriesTable_TaskId",
                table: "TaskHistoriesTable",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistoriesTable_TasksTable_TaskId",
                table: "TaskHistoriesTable",
                column: "TaskId",
                principalTable: "TasksTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistoriesTable_TasksTable_TaskId",
                table: "TaskHistoriesTable");

            migrationBuilder.DropIndex(
                name: "IX_TaskHistoriesTable_TaskId",
                table: "TaskHistoriesTable");

            migrationBuilder.AddColumn<long>(
                name: "CompletedTaskID",
                table: "TaskHistoriesTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistoriesTable_CompletedTaskID",
                table: "TaskHistoriesTable",
                column: "CompletedTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistoriesTable_TasksTable_CompletedTaskID",
                table: "TaskHistoriesTable",
                column: "CompletedTaskID",
                principalTable: "TasksTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
