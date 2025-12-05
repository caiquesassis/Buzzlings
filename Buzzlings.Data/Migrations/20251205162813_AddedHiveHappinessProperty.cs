using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buzzlings.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedHiveHappinessProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Happiness",
                table: "Hives",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Happiness",
                table: "Hives");
        }
    }
}
