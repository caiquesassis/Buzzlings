using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buzzlings.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTopHivesCascadingDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HiveId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HiveId",
                table: "AspNetUsers",
                column: "HiveId",
                unique: true,
                filter: "[HiveId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HiveId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HiveId",
                table: "AspNetUsers",
                column: "HiveId");
        }
    }
}
