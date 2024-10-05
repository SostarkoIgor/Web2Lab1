using Microsoft.EntityFrameworkCore;
using Web2Lab1.Server.Models;

namespace Web2Lab1.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
