using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class userScoreAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Currency",
                table: "UsersTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalScore",
                table: "UsersTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Currency",
                table: "GamesTable",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "UsersTable");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Currency",
                table: "GamesTable",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
