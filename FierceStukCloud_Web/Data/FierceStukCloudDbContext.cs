using FierceStukCloud.Core;
using Microsoft.EntityFrameworkCore;

namespace FierceStukCloud_Web.Data
{
    public class FierceStukCloudDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }

        public DbSet<User> Users { get; set; }

        public FierceStukCloudDbContext(DbContextOptions<FierceStukCloudDbContext> options) : base(options)
        {

        }

    }
}
