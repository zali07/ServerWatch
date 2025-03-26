using Microsoft.EntityFrameworkCore;
using ServerWatchWS.Model;

namespace ServerWatchWS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MirroringData> MirroringEntries { get; set; }
        public DbSet<DriverData> DriverEntries { get; set; }
        public DbSet<Servers> Servers { get; set; }
    }
}
