using FierceStukCloud.Core;
using FierceStukCloud.Core.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud.EntityFramework
{
    public class FierceStukCloudDbContext :DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        //public DbSet<Album> Albums { get; set; }
        //public DbSet<Author> Authors { get; set; }
        //public DbSet<LocalFolder> LocalFolders { get; set; }

        public FierceStukCloudDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ObservableLinkedList<Song>>().Ignore("Songs");

            modelBuilder.Entity<SongPlayList>()
            .HasKey(t => new { t.SongId, t.PlayListId });

            modelBuilder.Entity<SongPlayList>()
                .HasOne(spl => spl.Song)
                .WithMany(s => s.PlayLists)
                .HasForeignKey(spl => spl.SongId);

            modelBuilder.Entity<SongPlayList>()
                .HasOne(spl => spl.PlayList)
                .WithMany(c => c.DbSongs)
                .HasForeignKey(spl => spl.PlayListId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
