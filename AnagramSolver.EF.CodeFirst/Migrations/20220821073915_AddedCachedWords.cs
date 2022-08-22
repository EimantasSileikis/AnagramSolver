using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.EF.CodeFirst.Migrations
{
    public partial class AddedCachedWords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anagrams_CachedWords_WordId",
                table: "Anagrams");

            migrationBuilder.DropTable(
                name: "SearchHistories");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "Anagrams",
                newName: "CachedWordId");

            migrationBuilder.RenameColumn(
                name: "Anagram",
                table: "Anagrams",
                newName: "Word");

            migrationBuilder.RenameIndex(
                name: "IX_Anagrams_WordId",
                table: "Anagrams",
                newName: "IX_Anagrams_CachedWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Anagrams_CachedWords_CachedWordId",
                table: "Anagrams",
                column: "CachedWordId",
                principalTable: "CachedWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anagrams_CachedWords_CachedWordId",
                table: "Anagrams");

            migrationBuilder.RenameColumn(
                name: "Word",
                table: "Anagrams",
                newName: "Anagram");

            migrationBuilder.RenameColumn(
                name: "CachedWordId",
                table: "Anagrams",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_Anagrams_CachedWordId",
                table: "Anagrams",
                newName: "IX_Anagrams_WordId");

            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anagrams = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSpent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Anagrams_CachedWords_WordId",
                table: "Anagrams",
                column: "WordId",
                principalTable: "CachedWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
