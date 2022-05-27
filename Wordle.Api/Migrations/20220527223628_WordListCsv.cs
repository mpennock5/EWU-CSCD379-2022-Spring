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

            migrationBuilder.AlterColumn<int>(
                name: "DateWordId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "Common",
                table: "Words");

            migrationBuilder.AlterColumn<int>(
                name: "DateWordId",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DateWords_DateWordId",
                table: "Games",
                column: "DateWordId",
                principalTable: "DateWords",
                principalColumn: "DateWordId");
        }
    }
}
