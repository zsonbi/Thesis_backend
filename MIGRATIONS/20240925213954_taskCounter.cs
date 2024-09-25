using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class taskCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedBadTasks",
                table: "UsersTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompletedGoodTasks",
                table: "UsersTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedBadTasks",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "CompletedGoodTasks",
                table: "UsersTable");
        }
    }
}
