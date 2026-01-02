using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_Crystal_In_UserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Crystal",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crystal",
                table: "UserInfos");
        }
    }
}
