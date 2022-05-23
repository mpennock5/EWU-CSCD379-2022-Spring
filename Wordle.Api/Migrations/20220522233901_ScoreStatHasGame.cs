using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    public partial class ScoreStatHasGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "ScoreStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScoreStatId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ScoreStatId",
                table: "Games",
                column: "ScoreStatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ScoreStats_ScoreStatId",
                table: "Games",
                column: "ScoreStatId",
                principalTable: "ScoreStats",
                principalColumn: "ScoreStatId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_ScoreStats_ScoreStatId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ScoreStatId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "ScoreStats");

            migrationBuilder.DropColumn(
                name: "ScoreStatId",
                table: "Games");
        }
    }
}
