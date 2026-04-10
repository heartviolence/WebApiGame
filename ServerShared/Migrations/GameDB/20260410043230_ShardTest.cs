using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.GameDB
{
    /// <inheritdoc />
    public partial class ShardTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AchievementsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GachaCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Crystal = table.Column<int>(type: "int", nullable: false),
                    AchievementDataId = table.Column<int>(type: "int", nullable: false),
                    GameState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceievedUserRewards = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserDetails_AchievementsData_AchievementDataId",
                        column: x => x.AchievementDataId,
                        principalTable: "AchievementsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedAchievement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    RewardCheckPoint = table.Column<int>(type: "int", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedAchievement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedAchievement_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GameCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    EXP = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    A_Skill_Level = table.Column<int>(type: "int", nullable: false),
                    B_Skill_Level = table.Column<int>(type: "int", nullable: false),
                    StarLevel = table.Column<int>(type: "int", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCharacters_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RecordItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StarLevel = table.Column<int>(type: "int", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordItem_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RequestMissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMissions_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserMail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMail_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    UserAccountDetailUserId = table.Column<int>(type: "int", nullable: true),
                    UserMailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameItem_UserDetails_UserAccountDetailUserId",
                        column: x => x.UserAccountDetailUserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_GameItem_UserMail_UserMailId",
                        column: x => x.UserMailId,
                        principalTable: "UserMail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedAchievement_UserAccountDetailUserId",
                table: "CompletedAchievement",
                column: "UserAccountDetailUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCharacters_UserAccountDetailUserId",
                table: "GameCharacters",
                column: "UserAccountDetailUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_UserAccountDetailUserId",
                table: "GameItem",
                column: "UserAccountDetailUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_UserMailId",
                table: "GameItem",
                column: "UserMailId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordItem_UserAccountDetailUserId",
                table: "RecordItem",
                column: "UserAccountDetailUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMissions_UserAccountDetailUserId",
                table: "RequestMissions",
                column: "UserAccountDetailUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_AchievementDataId",
                table: "UserDetails",
                column: "AchievementDataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_Username",
                table: "UserDetails",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMail_UserAccountDetailUserId",
                table: "UserMail",
                column: "UserAccountDetailUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedAchievement");

            migrationBuilder.DropTable(
                name: "GameCharacters");

            migrationBuilder.DropTable(
                name: "GameEvents");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "RecordItem");

            migrationBuilder.DropTable(
                name: "RequestMissions");

            migrationBuilder.DropTable(
                name: "UserMail");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "AchievementsData");
        }
    }
}
