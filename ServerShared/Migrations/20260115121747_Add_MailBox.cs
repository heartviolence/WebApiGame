using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations
{
    /// <inheritdoc />
    public partial class Add_MailBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "GameItem");

            migrationBuilder.AddColumn<string>(
                name: "ReceievedUserRewards",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "UserMailId",
                table: "GameItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRewardId",
                table: "GameItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserMail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMail_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_UserMailId",
                table: "GameItem",
                column: "UserMailId");

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_UserRewardId",
                table: "GameItem",
                column: "UserRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMail_UserInfoId",
                table: "UserMail",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_UserMail_UserMailId",
                table: "GameItem",
                column: "UserMailId",
                principalTable: "UserMail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_UserRewards_UserRewardId",
                table: "GameItem",
                column: "UserRewardId",
                principalTable: "UserRewards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_UserMail_UserMailId",
                table: "GameItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_UserRewards_UserRewardId",
                table: "GameItem");

            migrationBuilder.DropTable(
                name: "UserMail");

            migrationBuilder.DropTable(
                name: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_GameItem_UserMailId",
                table: "GameItem");

            migrationBuilder.DropIndex(
                name: "IX_GameItem_UserRewardId",
                table: "GameItem");

            migrationBuilder.DropColumn(
                name: "ReceievedUserRewards",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "UserMailId",
                table: "GameItem");

            migrationBuilder.DropColumn(
                name: "UserRewardId",
                table: "GameItem");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "GameItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
