using FierceStukCloud.Core;
using FierceStukCloud.Core.Extension.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace FierceStukCloud.EntityFramework
{
    public class FierceStukCloudDbContext :DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<LocalFolder> LocalFolders { get; set; }

        public FierceStukCloudDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ObservableLinkedList<Song>>().Ignore("Songs");

            #region SongPlayList
            modelBuilder.Entity<SongPlayList>()
                .HasKey(t => new { t.SongId, t.PlayListId });

            modelBuilder.Entity<SongPlayList>()
                .HasOne(spl => spl.Song)
                .WithMany(s => s.DbPlayLists)
                .HasForeignKey(spl => spl.SongId);

            modelBuilder.Entity<SongPlayList>()
                .HasOne(spl => spl.PlayList)
                .WithMany(c => c.DbSongs)
                .HasForeignKey(spl => spl.PlayListId);
            #endregion

            #region SongAuthor
            modelBuilder.Entity<SongAuthor>()
                .HasKey(t => new { t.SongId, t.AuthorId });

            modelBuilder.Entity<SongAuthor>()
                .HasOne(sa => sa.Song)
                .WithMany(s => s.DbAuthors)
                .HasForeignKey(sa => sa.SongId);

            modelBuilder.Entity<SongAuthor>()
                .HasOne(sa => sa.Author)
                .WithMany(a => a.DbSongs)
                .HasForeignKey(sa => sa.AuthorId);
            #endregion

            #region SongAlbum
            modelBuilder.Entity<SongAlbum>()
               .HasKey(t => new { t.SongId, t.AlbumId });

            modelBuilder.Entity<SongAlbum>()
                .HasOne(sa => sa.Song)
                .WithMany(s => s.DbAlbums)
                .HasForeignKey(sa => sa.SongId);

            modelBuilder.Entity<SongAlbum>()
                .HasOne(sa => sa.Album)
                .WithMany(a => a.DbSongs)
                .HasForeignKey(sa => sa.AlbumId);
            #endregion


            #region AlbumAuthor
            modelBuilder.Entity<AlbumAuthor>()
               .HasKey(t => new { t.AlbumId, t.AuthorId });

            modelBuilder.Entity<AlbumAuthor>()
                .HasOne(sa => sa.Album)
                .WithMany(s => s.DbAuthors)
                .HasForeignKey(sa => sa.AlbumId);

            modelBuilder.Entity<AlbumAuthor>()
                .HasOne(sa => sa.Author)
                .WithMany(a => a.DbAlbums)
                .HasForeignKey(sa => sa.AuthorId);
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
