using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    public partial class WordListCsv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "DateWordId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games",
                column: "DateWordId",
                principalTable: "DateWords",
                principalColumn: "DateWordId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "DateWordId",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games",
                column: "DateWordId",
                principalTable: "DateWords",
                principalColumn: "DateWordId");
        }
    }
}
