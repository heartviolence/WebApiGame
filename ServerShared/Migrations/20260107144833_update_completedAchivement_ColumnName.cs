using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations
{
    /// <inheritdoc />
    public partial class update_completedAchivement_ColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AchievementCode",
                table: "CompletedAchievement",
                newName: "AchievementName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AchievementName",
                table: "CompletedAchievement",
                newName: "AchievementCode");
        }
    }
}
