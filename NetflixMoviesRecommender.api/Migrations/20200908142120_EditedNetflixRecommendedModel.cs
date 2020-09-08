using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class EditedNetflixRecommendedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "NetflixRecommendations",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "NetflixRecommendations");
        }
    }
}
