using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.GameDB
{
    /// <inheritdoc />
    public partial class AddForeignKeyinUserDetailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedAchievement_UserDetails_UserAccountDetailUserId",
                table: "CompletedAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMail_UserDetails_UserAccountDetailUserId",
                table: "UserMail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMissions",
                table: "RequestMissions");

            migrationBuilder.RenameTable(
                name: "RequestMissions",
                newName: "RequestMission");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMissions_UserAccountDetailUserId",
                table: "RequestMission",
                newName: "IX_RequestMission_UserAccountDetailUserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "UserMail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "CompletedAchievement",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMission",
                table: "RequestMission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedAchievement_UserDetails_UserAccountDetailUserId",
                table: "CompletedAchievement",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMission_UserDetails_UserAccountDetailUserId",
                table: "RequestMission",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMail_UserDetails_UserAccountDetailUserId",
                table: "UserMail",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedAchievement_UserDetails_UserAccountDetailUserId",
                table: "CompletedAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMission_UserDetails_UserAccountDetailUserId",
                table: "RequestMission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMail_UserDetails_UserAccountDetailUserId",
                table: "UserMail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMission",
                table: "RequestMission");

            migrationBuilder.RenameTable(
                name: "RequestMission",
                newName: "RequestMissions");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMission_UserAccountDetailUserId",
                table: "RequestMissions",
                newName: "IX_RequestMissions_UserAccountDetailUserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "UserMail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountDetailUserId",
                table: "CompletedAchievement",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMissions",
                table: "RequestMissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedAchievement_UserDetails_UserAccountDetailUserId",
                table: "CompletedAchievement",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                table: "RequestMissions",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMail_UserDetails_UserAccountDetailUserId",
                table: "UserMail",
                column: "UserAccountDetailUserId",
                principalTable: "UserDetails",
                principalColumn: "UserId");
        }
    }
}
