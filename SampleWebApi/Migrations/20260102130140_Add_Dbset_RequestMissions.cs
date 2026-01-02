using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_Dbset_RequestMissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestMission_UserInfos_UserInfoId",
                table: "RequestMission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMission",
                table: "RequestMission");

            migrationBuilder.RenameTable(
                name: "RequestMission",
                newName: "RequestMissions");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMission_UserInfoId",
                table: "RequestMissions",
                newName: "IX_RequestMissions_UserInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMissions",
                table: "RequestMissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMissions_UserInfos_UserInfoId",
                table: "RequestMissions",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestMissions_UserInfos_UserInfoId",
                table: "RequestMissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMissions",
                table: "RequestMissions");

            migrationBuilder.RenameTable(
                name: "RequestMissions",
                newName: "RequestMission");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMissions_UserInfoId",
                table: "RequestMission",
                newName: "IX_RequestMission_UserInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMission",
                table: "RequestMission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMission_UserInfos_UserInfoId",
                table: "RequestMission",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }
    }
}
