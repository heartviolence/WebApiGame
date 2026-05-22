using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.GameDB
{
    /// <inheritdoc />
    public partial class AddGameItemDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_UserDetails_UserAccountDetailUserId",
                table: "GameItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_UserMail_UserMailId",
                table: "GameItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameItem",
                table: "GameItem");

            migrationBuilder.RenameTable(
                name: "GameItem",
                newName: "GameItems");

            migrationBuilder.RenameIndex(
                name: "IX_GameItem_UserMailId",
                table: "GameItems",
                newName: "IX_GameItems_UserMailId");

            migrationBuilder.RenameIndex(
                name: "IX_GameItem_UserAccountDetailUserId",
                table: "GameItems",
                newName: "IX_GameItems_UserAccountDetailUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameItems",
                table: "GameItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItems_UserDetails_UserAccountDetailUserId",
                table: "GameItems",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItems_UserMail_UserMailId",
                table: "GameItems",
                column: "UserMailId",
                principalTable: "UserMail",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameItems_UserDetails_UserAccountDetailUserId",
                table: "GameItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GameItems_UserMail_UserMailId",
                table: "GameItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameItems",
                table: "GameItems");

            migrationBuilder.RenameTable(
                name: "GameItems",
                newName: "GameItem");

            migrationBuilder.RenameIndex(
                name: "IX_GameItems_UserMailId",
                table: "GameItem",
                newName: "IX_GameItem_UserMailId");

            migrationBuilder.RenameIndex(
                name: "IX_GameItems_UserAccountDetailUserId",
                table: "GameItem",
                newName: "IX_GameItem_UserAccountDetailUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameItem",
                table: "GameItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_UserDetails_UserAccountDetailUserId",
                table: "GameItem",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_UserMail_UserMailId",
                table: "GameItem",
                column: "UserMailId",
                principalTable: "UserMail",
                principalColumn: "Id");
        }
    }
}
