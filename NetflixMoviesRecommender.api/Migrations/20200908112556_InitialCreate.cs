using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Year = table.Column<string>(maxLength: 10, nullable: true),
                    Rated = table.Column<string>(maxLength: 10, nullable: true),
                    Released = table.Column<DateTime>(nullable: false),
                    Runtime = table.Column<string>(maxLength: 20, nullable: true),
                    Genre = table.Column<string>(maxLength: 100, nullable: true),
                    Director = table.Column<string>(maxLength: 100, nullable: true),
                    Writer = table.Column<string>(maxLength: 100, nullable: true),
                    Actors = table.Column<string>(nullable: true),
                    Plot = table.Column<string>(nullable: true),
                    Language = table.Column<string>(maxLength: 100, nullable: true),
                    Country = table.Column<string>(maxLength: 100, nullable: true),
                    Awards = table.Column<string>(maxLength: 50, nullable: true),
                    Poster = table.Column<string>(maxLength: 500, nullable: true),
                    Metascore = table.Column<string>(maxLength: 20, nullable: true),
                    imdbRating = table.Column<string>(maxLength: 10, nullable: true),
                    imdbVotes = table.Column<string>(maxLength: 10, nullable: true),
                    imdbID = table.Column<string>(maxLength: 20, nullable: true),
                    Type = table.Column<string>(maxLength: 20, nullable: true),
                    totalSeasons = table.Column<string>(maxLength: 10, nullable: true),
                    Response = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Type = table.Column<string>(maxLength: 10, nullable: true),
                    DateWatched = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(maxLength: 20, nullable: true),
                    RecommendedForeignKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Recommendations_RecommendedForeignKey",
                        column: x => x.RecommendedForeignKey,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_RecommendedForeignKey",
                table: "Rating",
                column: "RecommendedForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "WatchItems");

            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
