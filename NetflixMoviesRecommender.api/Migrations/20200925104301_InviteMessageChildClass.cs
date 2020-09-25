using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class InviteMessageChildClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "InboxMessages",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "InboxMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupTitle",
                table: "InboxMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "GroupTitle",
                table: "InboxMessages");
        }
    }
}
