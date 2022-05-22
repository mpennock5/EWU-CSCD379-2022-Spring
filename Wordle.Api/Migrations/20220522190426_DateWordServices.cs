using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    public partial class DateWordServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateWordId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DateWordId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageGuesses",
                table: "DateWords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AverageSeconds",
                table: "DateWords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPlays",
                table: "DateWords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_DateWordId",
                table: "Players",
                column: "DateWordId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DateWordId",
                table: "Games",
                column: "DateWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games",
                column: "DateWordId",
                principalTable: "DateWords",
                principalColumn: "DateWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_DateWords_DateWordId",
                table: "Players",
                column: "DateWordId",
                principalTable: "DateWords",
                principalColumn: "DateWordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_DateWords_DateWordId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_DateWordId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Games_DateWordId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DateWordId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DateWordId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "AverageGuesses",
                table: "DateWords");

            migrationBuilder.DropColumn(
                name: "AverageSeconds",
                table: "DateWords");

            migrationBuilder.DropColumn(
                name: "TotalPlays",
                table: "DateWords");
        }
    }
}
