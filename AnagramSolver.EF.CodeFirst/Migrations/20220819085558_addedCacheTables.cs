using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.EF.CodeFirst.Migrations
{
    public partial class addedCacheTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anagrams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anagrams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anagrams_CachedWords_WordId",
                        column: x => x.WordId,
                        principalTable: "CachedWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anagrams_WordId",
                table: "Anagrams",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anagrams");

            migrationBuilder.DropTable(
                name: "CachedWords");
        }
    }
}
