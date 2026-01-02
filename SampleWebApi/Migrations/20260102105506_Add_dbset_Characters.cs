using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbset_Characters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharacter_UserInfos_UserInfoId",
                table: "GameCharacter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCharacter",
                table: "GameCharacter");

            migrationBuilder.RenameTable(
                name: "GameCharacter",
                newName: "GameCharacters");

            migrationBuilder.RenameIndex(
                name: "IX_GameCharacter_UserInfoId",
                table: "GameCharacters",
                newName: "IX_GameCharacters_UserInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCharacters",
                table: "GameCharacters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharacters_UserInfos_UserInfoId",
                table: "GameCharacters",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharacters_UserInfos_UserInfoId",
                table: "GameCharacters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCharacters",
                table: "GameCharacters");

            migrationBuilder.RenameTable(
                name: "GameCharacters",
                newName: "GameCharacter");

            migrationBuilder.RenameIndex(
                name: "IX_GameCharacters_UserInfoId",
                table: "GameCharacter",
                newName: "IX_GameCharacter_UserInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCharacter",
                table: "GameCharacter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharacter_UserInfos_UserInfoId",
                table: "GameCharacter",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }
    }
}
