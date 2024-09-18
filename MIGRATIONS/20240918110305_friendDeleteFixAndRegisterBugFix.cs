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
                name: "FK_FriendsTable_UsersTable_ReceiverID",
                table: "FriendsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderID",
                table: "FriendsTable");

            migrationBuilder.DropIndex(
                name: "IX_FriendsTable_SenderID",
                table: "FriendsTable");

            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "FriendsTable",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "FriendsTable",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendsTable_ReceiverID",
                table: "FriendsTable",
                newName: "IX_FriendsTable_ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_SenderId_ReceiverId",
                table: "FriendsTable",
                columns: new[] { "SenderId", "ReceiverId" },
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
                name: "IX_FriendsTable_SenderId_ReceiverId",
                table: "FriendsTable");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "FriendsTable",
                newName: "SenderID");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "FriendsTable",
                newName: "ReceiverID");

            migrationBuilder.RenameIndex(
                name: "IX_FriendsTable_ReceiverId",
                table: "FriendsTable",
                newName: "IX_FriendsTable_ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsTable_SenderID",
                table: "FriendsTable",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_ReceiverID",
                table: "FriendsTable",
                column: "ReceiverID",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsTable_UsersTable_SenderID",
                table: "FriendsTable",
                column: "SenderID",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
