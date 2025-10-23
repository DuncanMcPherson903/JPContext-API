using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPContext.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDataToExamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Examples",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Examples");
        }
    }
}
