using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    public partial class PleaseWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guess");

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "WordId",
                keyValue: 6);

            migrationBuilder.AddColumn<bool>(
                name: "Common",
                table: "Words",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "Common",
                table: "Words");

            migrationBuilder.CreateTable(
                name: "Guess",
                columns: table => new
                {
                    GuessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guess", x => x.GuessId);
                    table.ForeignKey(
                        name: "FK_Guess_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "WordId", "Value" },
                values: new object[,]
                {
                    { 1, "thing" },
                    { 2, "think" },
                    { 3, "thong" },
                    { 4, "throb" },
                    { 5, "thunk" },
                    { 6, "wrong" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guess_GameId",
                table: "Guess",
                column: "GameId");
        }
    }
}
