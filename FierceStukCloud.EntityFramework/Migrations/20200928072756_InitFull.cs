using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FierceStukCloud.EntityFramework.Migrations
{
    public partial class InitFull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<long>(nullable: false),
                    UserLogin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    LocalUrl = table.Column<string>(nullable: true),
                    UserLogin = table.Column<string>(nullable: true),
                    OnServer = table.Column<bool>(nullable: false),
                    OnDevice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalFolders", x => x.Id);
                });

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
                name: "AlbumAuthor",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumAuthor", x => new { x.AlbumId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_AlbumAuthor_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumAuthor_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<long>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    LocalUrl = table.Column<string>(nullable: true),
                    LocalFolderId = table.Column<Guid>(nullable: true),
                    UserLogin = table.Column<string>(nullable: true),
                    OnServer = table.Column<bool>(nullable: false),
                    OnDevice = table.Column<bool>(nullable: false),
                    OptionalInfo = table.Column<string>(nullable: true),
                    Favorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_LocalFolders_LocalFolderId",
                        column: x => x.LocalFolderId,
                        principalTable: "LocalFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SongAlbum",
                columns: table => new
                {
                    SongId = table.Column<Guid>(nullable: false),
                    AlbumId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongAlbum", x => new { x.SongId, x.AlbumId });
                    table.ForeignKey(
                        name: "FK_SongAlbum_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongAlbum_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SongAuthor",
                columns: table => new
                {
                    SongId = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongAuthor", x => new { x.SongId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_SongAuthor_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongAuthor_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AlbumAuthor_AuthorId",
                table: "AlbumAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_SongAlbum_AlbumId",
                table: "SongAlbum",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_SongAuthor_AuthorId",
                table: "SongAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_SongPlayList_PlayListId",
                table: "SongPlayList",
                column: "PlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_LocalFolderId",
                table: "Songs",
                column: "LocalFolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumAuthor");

            migrationBuilder.DropTable(
                name: "SongAlbum");

            migrationBuilder.DropTable(
                name: "SongAuthor");

            migrationBuilder.DropTable(
                name: "SongPlayList");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "PlayLists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "LocalFolders");
        }
    }
}
