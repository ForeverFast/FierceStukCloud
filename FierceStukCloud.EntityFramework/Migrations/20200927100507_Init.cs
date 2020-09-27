using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FierceStukCloud.EntityFramework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UserLogin = table.Column<string>(nullable: true),
                    OnServer = table.Column<bool>(nullable: false),
                    OnDevice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Album = table.Column<string>(nullable: true),
                    Year = table.Column<long>(nullable: false),
                    Duration = table.Column<string>(nullable: true),
                    LocalUrl = table.Column<string>(nullable: true),
                    UserLogin = table.Column<string>(nullable: true),
                    OnServer = table.Column<bool>(nullable: false),
                    OnDevice = table.Column<bool>(nullable: false),
                    OptionalInfo = table.Column<string>(nullable: true),
                    Favorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SongPlayList",
                columns: table => new
                {
                    SongId = table.Column<Guid>(nullable: false),
                    PlayListId = table.Column<Guid>(nullable: false),
                    Place = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongPlayList", x => new { x.SongId, x.PlayListId });
                    table.ForeignKey(
                        name: "FK_SongPlayList_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongPlayList_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongPlayList_PlayListId",
                table: "SongPlayList",
                column: "PlayListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongPlayList");

            migrationBuilder.DropTable(
                name: "PlayLists");

            migrationBuilder.DropTable(
                name: "Songs");
        }
    }
}
