using Microsoft.EntityFrameworkCore;
using OpenX.Models;

namespace OpenX.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserDetails> Users { get; set; } = null!;

    }
}
