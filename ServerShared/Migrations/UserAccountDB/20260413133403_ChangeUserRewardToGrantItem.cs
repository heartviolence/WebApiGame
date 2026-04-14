using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.UserAccountDB
{
    /// <inheritdoc />
    public partial class ChangeUserRewardToGrantItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_UserRewards_UserRewardId",
                table: "GameItem");

            migrationBuilder.DropTable(
                name: "UserRewards");

            migrationBuilder.RenameColumn(
                name: "UserRewardId",
                table: "GameItem",
                newName: "GrantItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GameItem_UserRewardId",
                table: "GameItem",
                newName: "IX_GameItem_GrantItemId");

            migrationBuilder.CreateTable(
                name: "GrantItems",
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
                    table.PrimaryKey("PK_GrantItems", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_GrantItems_GrantItemId",
                table: "GameItem",
                column: "GrantItemId",
                principalTable: "GrantItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameItem_GrantItems_GrantItemId",
                table: "GameItem");

            migrationBuilder.DropTable(
                name: "GrantItems");

            migrationBuilder.RenameColumn(
                name: "GrantItemId",
                table: "GameItem",
                newName: "UserRewardId");

            migrationBuilder.RenameIndex(
                name: "IX_GameItem_GrantItemId",
                table: "GameItem",
                newName: "IX_GameItem_UserRewardId");

            migrationBuilder.CreateTable(
                name: "UserRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewards", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GameItem_UserRewards_UserRewardId",
                table: "GameItem",
                column: "UserRewardId",
                principalTable: "UserRewards",
                principalColumn: "Id");
        }
    }
}
