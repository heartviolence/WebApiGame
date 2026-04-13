using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerShared.Migrations.UserAccountDB
{
    /// <inheritdoc />
    public partial class AddGameEventShardColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Shard",
                table: "GameEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shard",
                table: "GameEvents");
        }
    }
}
