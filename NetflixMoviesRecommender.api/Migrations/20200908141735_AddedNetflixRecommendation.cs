using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class AddedNetflixRecommendation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetflixRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(maxLength: 10, nullable: true),
                    Genres = table.Column<string>(maxLength: 200, nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Plot = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetflixRecommendations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetflixRecommendations");
        }
    }
}
