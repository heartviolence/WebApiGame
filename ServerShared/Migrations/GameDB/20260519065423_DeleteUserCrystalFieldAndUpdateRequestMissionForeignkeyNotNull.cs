using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.GameDB
{
    /// <inheritdoc />
    public partial class DeleteUserCrystalFieldAndUpdateRequestMissionForeignkeyNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions");

            migrationBuilder.DropColumn(
                name: "Crystal",
                table: "UserDetails");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "RequestMissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions");

            migrationBuilder.AddColumn<int>(
                name: "Crystal",
                table: "UserDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "RequestMissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");
        }
    }
}
