using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class friendTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_UsersTable_RecieverID",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_UsersTable_SenderID",
                table: "Friend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "FriendsTable");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_SenderID",
                table: "FriendsTable",
                newName: "IX_FriendsTable_SenderID");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_RecieverID",
                table: "FriendsTable",
                newName: "IX_FriendsTable_RecieverID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendsTable",
                table: "FriendsTable",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_RecieverID",
                table: "FriendsTable",
                column: "RecieverID",
                principalTable: "UsersTable",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderID",
                table: "FriendsTable",
                column: "SenderID",
                principalTable: "UsersTable",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_RecieverID",
                table: "FriendsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderID",
                table: "FriendsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendsTable",
                table: "FriendsTable");

            migrationBuilder.RenameTable(
                name: "FriendsTable",
                newName: "Friend");

            migrationBuilder.RenameIndex(
                name: "IX_FriendsTable_SenderID",
                table: "Friend",
                newName: "IX_Friend_SenderID");

            migrationBuilder.RenameIndex(
                name: "IX_FriendsTable_RecieverID",
                table: "Friend",
                newName: "IX_Friend_RecieverID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friend",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_UsersTable_RecieverID",
                table: "Friend",
                column: "RecieverID",
                principalTable: "UsersTable",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_UsersTable_SenderID",
                table: "Friend",
                column: "SenderID",
                principalTable: "UsersTable",
                principalColumn: "ID");
        }
    }
}
