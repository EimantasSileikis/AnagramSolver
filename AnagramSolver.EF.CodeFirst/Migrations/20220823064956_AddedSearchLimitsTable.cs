using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.EF.CodeFirst.Migrations
{
    public partial class AddedSearchLimitsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchLimits",
                columns: table => new
                {
                    Ip = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Limit = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchLimits", x => x.Ip);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchLimits");
        }
    }
}
