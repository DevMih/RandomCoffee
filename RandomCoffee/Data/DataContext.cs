using Microsoft.EntityFrameworkCore;
using RandomCoffee.Entities;

namespace RandomCoffee.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
