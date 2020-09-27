using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud.EntityFramework
{
    public class FierceStukCloudDbContextFactory : IDesignTimeDbContextFactory<FierceStukCloudDbContext>
    {
        public FierceStukCloudDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<FierceStukCloudDbContext>();
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=FierceStukCloudDB; Trusted_Connection=True;");

            return new FierceStukCloudDbContext(options.Options);
        }
    }
}
