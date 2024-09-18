using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class friendDeleteFixAndRegisterBugFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_RecieverID",
                table: "FriendsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderID",
                table: "FriendsTable");

            migrationBuilder.DropIndex(
                name: "IX_FriendsTable_RecieverID",
                table: "FriendsTable");

            migrationBuilder.DropIndex(
                name: "IX_FriendsTable_SenderID",
                table: "FriendsTable");

            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "FriendsTable",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "RecieverID",
                table: "FriendsTable",
                newName: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_Username_Email",
                table: "UsersTable",
                columns: new[] { "Username", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_ReceiverId",
                table: "FriendsTable",
                column: "ReceiverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_SenderId",
                table: "FriendsTable",
                column: "SenderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_ReceiverId",
                table: "FriendsTable",
                column: "ReceiverId",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderId",
                table: "FriendsTable",
                column: "SenderId",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_ReceiverId",
                table: "FriendsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderId",
                table: "FriendsTable");

            migrationBuilder.DropIndex(
                name: "IX_UsersTable_Username_Email",
                table: "UsersTable");

            migrationBuilder.DropIndex(
                name: "IX_FriendsTable_ReceiverId",
                table: "FriendsTable");

            migrationBuilder.DropIndex(
                name: "IX_FriendsTable_SenderId",
                table: "FriendsTable");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "FriendsTable",
                newName: "SenderID");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "FriendsTable",
                newName: "RecieverID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_RecieverID",
                table: "FriendsTable",
                column: "RecieverID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_SenderID",
                table: "FriendsTable",
                column: "SenderID");

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
    }
}
