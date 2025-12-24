using Microsoft.EntityFrameworkCore;
using OpenX.AuthService.Models;

namespace OpenX.AuthService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserDetails> Users { get; set; } = null!;

    }
}
