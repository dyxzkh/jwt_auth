using asp.net_jwt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_jwt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<User> Users {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
