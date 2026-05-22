using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.GameDB
{
    /// <inheritdoc />
    public partial class gameCharacterForeignkeyIsNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharacters_UserDetails_UserAccountDetailUserId",
                table: "GameCharacters");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "GameCharacters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharacters_UserDetails_UserAccountDetailUserId",
                table: "GameCharacters",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharacters_UserDetails_UserAccountDetailUserId",
                table: "GameCharacters");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "GameCharacters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharacters_UserDetails_UserAccountDetailUserId",
                table: "GameCharacters",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");
        }
    }
}
