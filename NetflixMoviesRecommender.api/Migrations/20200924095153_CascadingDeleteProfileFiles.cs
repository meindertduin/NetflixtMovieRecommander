using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class CascadingDeleteProfileFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFiles_UserProfiles_UserProfileId",
                table: "ProfileFiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "ProfileFiles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFiles_UserProfiles_UserProfileId",
                table: "ProfileFiles",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFiles_UserProfiles_UserProfileId",
                table: "ProfileFiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "ProfileFiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFiles_UserProfiles_UserProfileId",
                table: "ProfileFiles",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
