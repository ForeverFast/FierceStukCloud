using Microsoft.EntityFrameworkCore.Migrations;

namespace FierceStukCloud.EntityFramework.Migrations
{
    public partial class Init_AddPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "SongAuthor",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "SongAlbum",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "AlbumAuthor",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "SongAuthor");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "SongAlbum");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "AlbumAuthor");
        }
    }
}
