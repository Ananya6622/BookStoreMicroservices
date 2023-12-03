using BookStore.Admin.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Admin.Context
{
    public class AdminDbContext: DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }
        public DbSet<AdminEntity> AdminTable { get; set; }
    }
}
