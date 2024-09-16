using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class friendTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SenderID = table.Column<long>(type: "bigint", nullable: true),
                    RecieverID = table.Column<long>(type: "bigint", nullable: true),
                    SentTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pending = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Friend_UsersTable_RecieverID",
                        column: x => x.RecieverID,
                        principalTable: "UsersTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Friend_UsersTable_SenderID",
                        column: x => x.SenderID,
                        principalTable: "UsersTable",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_RecieverID",
                table: "Friend",
                column: "RecieverID");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_SenderID",
                table: "Friend",
                column: "SenderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");
        }
    }
}
