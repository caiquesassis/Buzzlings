using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buzzlings.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetupHiveCascadingDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buzzlings_Hives_HiveId",
                table: "Buzzlings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hives",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddForeignKey(
                name: "FK_Buzzlings_Hives_HiveId",
                table: "Buzzlings",
                column: "HiveId",
                principalTable: "Hives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buzzlings_Hives_HiveId",
                table: "Buzzlings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hives",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_Buzzlings_Hives_HiveId",
                table: "Buzzlings",
                column: "HiveId",
                principalTable: "Hives",
                principalColumn: "Id");
        }
    }
}
