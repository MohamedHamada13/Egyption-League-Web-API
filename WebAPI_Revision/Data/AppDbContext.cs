using Microsoft.EntityFrameworkCore;
using WebAPI_Revision.Entities;

namespace WebAPI_Revision.Data
{
    public class AppDbContext : DbContext
    {
        // Important to configure Db Provider for applying migration.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        // We can Create the 'OnConfiguring()' method here for configuring the StringConnection, But it is more better to use regesteration method.
    }
}
