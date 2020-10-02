using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetflixMoviesRecommender.api.Migrations
{
    public partial class ProfileFilesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserProfiles");

            migrationBuilder.CreateTable(
                name: "ProfileFiles",
                columns: table => new
                {
                    ProfileFileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(maxLength: 200, nullable: true),
                    ContentType = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    FileType = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileFiles", x => x.ProfileFileId);
                    table.ForeignKey(
                        name: "FK_ProfileFiles_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileFiles_UserProfileId",
                table: "ProfileFiles",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileFiles");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
