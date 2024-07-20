using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis_backend.Migrations
{
    /// <inheritdoc />
    public partial class usersettingsAndTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersTable_Username_Email",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "UsersTable");

            migrationBuilder.RenameColumn(
                name: "TaskOwner",
                table: "TasksTable",
                newName: "TaskOwnerID");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePic",
                table: "UserSettingsTable",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserSettingsTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "TaskType",
                table: "TasksTable",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_Email",
                table: "UsersTable",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_Username",
                table: "UsersTable",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSettingsTable_UserId",
                table: "UserSettingsTable",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TasksTable_TaskOwnerID",
                table: "TasksTable",
                column: "TaskOwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_TasksTable_UsersTable_TaskOwnerID",
                table: "TasksTable",
                column: "TaskOwnerID",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettingsTable_UsersTable_UserId",
                table: "UserSettingsTable",
                column: "UserId",
                principalTable: "UsersTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TasksTable_UsersTable_TaskOwnerID",
                table: "TasksTable");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettingsTable_UsersTable_UserId",
                table: "UserSettingsTable");

            migrationBuilder.DropIndex(
                name: "IX_UsersTable_Email",
                table: "UsersTable");

            migrationBuilder.DropIndex(
                name: "IX_UsersTable_Username",
                table: "UsersTable");

            migrationBuilder.DropIndex(
                name: "IX_UserSettingsTable_UserId",
                table: "UserSettingsTable");

            migrationBuilder.DropIndex(
                name: "IX_TasksTable_TaskOwnerID",
                table: "TasksTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSettingsTable");

            migrationBuilder.DropColumn(
                name: "TaskType",
                table: "TasksTable");

            migrationBuilder.RenameColumn(
                name: "TaskOwnerID",
                table: "TasksTable",
                newName: "TaskOwner");

            migrationBuilder.AddColumn<long>(
                name: "SettingsId",
                table: "UsersTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "UserSettingsTable",
                keyColumn: "ProfilePic",
                keyValue: null,
                column: "ProfilePic",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePic",
                table: "UserSettingsTable",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_Username_Email",
                table: "UsersTable",
                columns: new[] { "Username", "Email" },
                unique: true);
        }
    }
}
