using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FierceStukCloud.EntityFramework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<long>(type: "bigint", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnServer = table.Column<bool>(type: "bit", nullable: false),
                    OnDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalFolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnServer = table.Column<bool>(type: "bit", nullable: false),
                    OnDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlbumAuthor",
                columns: table => new
                {
                    AlbumsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumAuthor", x => new { x.AlbumsId, x.AuthorsId });
                    table.ForeignKey(
                        name: "FK_AlbumAuthor_Albums_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumAuthor_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LocalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnServer = table.Column<bool>(type: "bit", nullable: false),
                    OnDevice = table.Column<bool>(type: "bit", nullable: false),
                    OptionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Favorite = table.Column<bool>(type: "bit", nullable: false),
                    PlayListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Songs_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlbumSong",
                columns: table => new
                {
                    AlbumsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumSong", x => new { x.AlbumsId, x.SongsId });
                    table.ForeignKey(
                        name: "FK_AlbumSong_Albums_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumSong_Songs_SongsId",
                        column: x => x.SongsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorSong",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorSong", x => new { x.AuthorsId, x.SongsId });
                    table.ForeignKey(
                        name: "FK_AuthorSong_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorSong_Songs_SongsId",
                        column: x => x.SongsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumAuthor_AuthorsId",
                table: "AlbumAuthor",
                column: "AuthorsId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSong_SongsId",
                table: "AlbumSong",
                column: "SongsId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSong_SongsId",
                table: "AuthorSong",
                column: "SongsId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_LocalFolderId",
                table: "Songs",
                column: "LocalFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlayListId",
                table: "Songs",
                column: "PlayListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumAuthor");

            migrationBuilder.DropTable(
                name: "AlbumSong");

            migrationBuilder.DropTable(
                name: "AuthorSong");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "LocalFolders");

            migrationBuilder.DropTable(
                name: "PlayLists");
        }
    }
}
