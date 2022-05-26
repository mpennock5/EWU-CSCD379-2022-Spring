using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    public partial class GameHasScoreStat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScoreStats",
                keyColumn: "ScoreStatId",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "AverageSeconds",
                table: "ScoreStats");

            migrationBuilder.RenameColumn(
                name: "TotalGames",
                table: "ScoreStats",
                newName: "Seconds");

            migrationBuilder.AddColumn<int>(
                name: "ScoreStatId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ScoreStatId",
                table: "Games",
                column: "ScoreStatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ScoreStats_ScoreStatId",
                table: "Games",
                column: "ScoreStatId",
                principalTable: "ScoreStats",
                principalColumn: "ScoreStatId");
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
                name: "ScoreStatId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Seconds",
                table: "ScoreStats",
                newName: "TotalGames");

            migrationBuilder.AddColumn<int>(
                name: "AverageSeconds",
                table: "ScoreStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "ScoreStats",
                columns: new[] { "ScoreStatId", "AverageSeconds", "Score", "TotalGames" },
                values: new object[,]
                {
                    { 1, 0, 1, 0 },
                    { 2, 0, 2, 0 },
                    { 3, 0, 3, 0 },
                    { 4, 0, 4, 0 },
                    { 5, 0, 5, 0 },
                    { 6, 0, 6, 0 }
                });
        }
    }
}
